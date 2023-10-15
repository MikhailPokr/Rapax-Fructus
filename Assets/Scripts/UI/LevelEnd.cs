using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RapaxFructus
{
    /// <summary>
    /// Скрипт меню окончания уровня.
    /// </summary>
    internal class LevelEnd : MonoBehaviour
    {
        [Serializable]
        public class Option
        {
            public GameObject Object;
            public PartType Size;
            public PartType Type;
            public int Num;
            private bool _active;
            private bool _select;
            public int Cost;

            public void SetActive(bool active)
            {
                _active = active;
                Rotate();
            }
            public void SetSelect(bool select)
            {
                _select = select;
                Rotate();
            }
            public void Rotate()
            {
                if (_select)
                {
                    if (!_active)
                    {
                        Object.transform.rotation = Quaternion.Euler(0, 0, 15);
                        return;
                    }
                }
                if (_active)
                {
                    Object.transform.rotation = Quaternion.Euler(0, 0, 45);
                    return;
                }
                if (!_select && !_active)
                {
                    Object.transform.rotation = Quaternion.Euler(0, 0, 0);
                    return;
                }
            }
        }

        [SerializeField] private GameObject _reset;
        [SerializeField] private Image _next;
        private Color _nextColor;
        [SerializeField] private Text _nextText;
        [SerializeField] private Image _menu;
        private Color _menuColor;
        [SerializeField] private GameObject _skip;
        [SerializeField] private Text _skipCounter;
        [SerializeField] private Option[] UsableOption;
        private int _choice = -2;

        private void OnEnable()
        {
            DataManager.ChangeEnergyPoints(Mathf.RoundToInt(ManagerDirectory.Instance.Player.CurrentEnergy), false);
            if (DataManager.EnergyPoints < 20)
                _reset.SetActive(true);
            else
                _reset.SetActive(false);
            _nextColor = _next.color;
            _menuColor = _menu.color;
            if (DataManager.SkipOptions <= 0)
            {
                _skip.SetActive(false);
            }
            else
            {
                _skipCounter.text = $"{DataManager.SkipOptions}/3";
            }
            ChangeChoice(-2);
        }

        public void Enter(int senderChoise)
        {
            foreach (Option option in UsableOption)
            {
                if (option.Num == senderChoise)
                    option.SetSelect(true);
                else
                    option.SetSelect(false);
            }
        }
        public void Exit(int senderChoise) 
        {
            foreach (Option option in UsableOption)
            {
                if (option.Num == senderChoise)
                    option.SetSelect(false);
            }
        }

        public void ChangeChoice(int choice)
        {
            _choice = choice;

            if (choice == -2)
            {
                _next.color = new Color(0.4f, 0.4f, 0.4f, _nextColor.a);
                _next.GetComponent<Button>().interactable = false;
                _menu.color = new Color(0.4f, 0.4f, 0.4f, _menuColor.a);
                _menu.GetComponent<Button>().interactable = false;
                foreach (Option option in UsableOption)
                {
                    option.SetActive(false);
                }
            }
            else
            {
                _next.color = _nextColor;
                _next.GetComponent<Button>().interactable = true;
                _menu.color = _menuColor;
                _menu.GetComponent<Button>().interactable = true;
                foreach (Option option in UsableOption)
                {
                    if (option.Num == choice)
                        option.SetActive(true);
                    else
                        option.SetActive(false);
                }

            }
        }
        private void ApplyChanges()
        {
            if (_choice != -1)
            {
                DataManager.ChangeSkipOptions(1);
                DataManager.Save.CurrentGameData.LevelMap.Add(UsableOption.First(x => x.Num == _choice).Size);
                DataManager.Save.CurrentGameData.EntranceMap.Add(UsableOption.First(x => x.Num == _choice).Type);
            }
            else
            {
                DataManager.ChangeSkipOptions(-1);
            }
            List<LevelFloor> levelFloors = ManagerDirectory.Instance.LevelGenerator.Floors;
            for (int i = 0; i < levelFloors.Count; i++)
            {
                if (levelFloors[i] == null)
                {
                    DataManager.Save.CurrentGameData.LevelMap[i] = PartType.Adapter;
                    DataManager.Save.CurrentGameData.EntranceMap[i] = PartType.Adapter;
                }
            }
            DataManager.Save.CurrentGameData.LevelMap.RemoveAll(x => x == PartType.Adapter);
            DataManager.Save.CurrentGameData.EntranceMap.RemoveAll(x => x == PartType.Adapter);
            DataManager.SaveAll();
        }
        public void ToMenu()
        {
            ApplyChanges();
            SceneHelper.ChangeScene(SceneHelper.Scene.Menu);
        }
        public void NextLevel()
        {
            ApplyChanges();
            int cost = ManagerDirectory.Instance.LevelGenerator.Cost;
            cost += UsableOption.First(x => x.Num == _choice).Cost;
            DataManager.ChangeEnergyPoints(-cost, false);
            SceneHelper.ChangeScene(SceneHelper.Scene.Game);
        }
        public void ResetProgress()
        {
            DataManager.DeleteAll();
            PlayerPrefs.DeleteAll();
            SceneHelper.ChangeScene(SceneHelper.Scene.Menu);
        }
    }
}