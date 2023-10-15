using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RapaxFructus
{
    internal abstract class Skill : MonoBehaviour
    {
        protected Image _button;
        [SerializeField] protected int _cost;
        [SerializeField] protected PowerUpObject _powerUpObject;
        public PowerUpObject PowerUpObject => _powerUpObject;

        public virtual void SetButton(Image button)
        {
            _button = button;
        }
        public virtual void Activate()
        {
            if (ManagerDirectory.Instance.Player.SpecialResourseCount - _cost < 0)
                return;
        }
        public void OnResourseCountChanged()
        {
            Color color = _button.color;
            if (ManagerDirectory.Instance.Player.SpecialResourseCount < _cost)
                color.a = 0.5f;
            else
                color.a = 1;
            _button.color = color;
        }
    }
}
