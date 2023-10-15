using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapaxFructus
{
    public class CoreHpDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject _model;
        [SerializeField] private RectTransform _hpCurrent;

        private bool _dead = false;
        public float SizeY => GetComponent<RectTransform>().rect.height;
        public void SetSize(PartType partType)
        {
            switch (partType)
            {
                case PartType.Tiny:
                    _model.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    break;
                case PartType.Little:
                    _model.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1, 1);
                    break;
                default:
                    return;
            }
        }
        public void SetHP(float hp, float maxHp)
        {
            float width = _hpCurrent.rect.width / 2;

            _hpCurrent.localScale = new Vector3(hp / maxHp, 1, 1);
            _hpCurrent.localPosition = new Vector3(-width + hp / maxHp * width, 0, 0);

            if (hp == 0)
                _dead = true;
        }
        public void Click()
        {
            if (_dead)
                return;
            ManagerDirectory.Instance.CameraMovement.ChangeLevel(ManagerDirectory.Instance.LevelGenerator.Floors.IndexOf(ManagerDirectory.Instance.CoreHpsDisplayManager.CoreDisplay.First(x => x.Value == this).Key.Floor) + 1);
            ManagerDirectory.Instance.CameraMovement.ChangePoint();
        }
    }
}