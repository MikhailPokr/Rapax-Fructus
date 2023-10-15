using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RapaxFructus
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Text _textStart;
        private int _cost;

        private void Start()
        {
            ControlSystem.Instance.ActionNotify += OnActionNotify;
            OnActionNotify(ControlSystem.Action._);
        }

        private void OnActionNotify(ControlSystem.Action action)
        {
            if (action == ControlSystem.Action.MoveRight || action == ControlSystem.Action.MoveLeft || action == ControlSystem.Action._)
            {
                LevelGenerator level = SaveFileManager.Instance.CurrentSaveFile.GetComponent<LevelGenerator>();
                _cost = 30;
                if (level != null)
                {
                    _cost = level.GetNecessaryEnergy();
                }
                _textStart.text = $"Start ({-_cost})";
            }
        }
        public void StartClick()
        {
            if (!DataManager.ChangeEnergyPoints(-_cost, false))
            {
                return;
            }
            SceneHelper.ChangeScene(SceneHelper.Scene.Game);
        }

        public void ExitClick()
        {
            Application.Quit();
        }
        public void PointerEnter(GameObject gameObject)
        {
            Vector3 pos = gameObject.transform.position;
            gameObject.transform.position = new Vector3(pos.x + 20, pos.y, pos.z);
        }
        public void PointerExit(GameObject gameObject)
        {
            Vector3 pos = gameObject.transform.position;
            gameObject.transform.position = new Vector3(pos.x - 20, pos.y, pos.z);
        }
    }
}