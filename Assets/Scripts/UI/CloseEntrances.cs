using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RapaxFructus
{
    public class CloseEntrances : MonoBehaviour
    {
        [SerializeField] private Image[] _fill;
        [SerializeField] private Color PointerInColorMod;
        [SerializeField] private Color EarlyExit;
        [SerializeField] private Color NormalExit;

        private bool _pointerIn;
        private bool _close;

        public delegate void CloseEvent();
        public event CloseEvent CloseNotify;
        public bool Close => _close;

        private void Start()
        {
            ManagerDirectory.Instance.Player.MinEnergyMarkCrossed += OnEnergyMarkCrossed;
        }

        private void OnEnergyMarkCrossed()
        {
            if (ManagerDirectory.Instance.Player.CurrentEnergy >= ManagerDirectory.Instance.Player.MinNecessaryEnergy)
                ChangeColor(NormalExit, 0);
            else
                ChangeColor(EarlyExit, 0);
            if (_pointerIn)
                ChangeColor(PointerInColorMod, 1);
        }
        private void ChangeColor(Color color, int modifier)
        {
            foreach (Image spriteRenderer in _fill)
            {
                if (modifier > 0)
                    spriteRenderer.color += color;
                else if (modifier < 0)
                    spriteRenderer.color -= color;
                else
                    spriteRenderer.color = color;
            }
        }

        public void Click()
        {
            if (_close)
                return;
            _close = true;
            CloseNotify?.Invoke();
        }
        public void Enter()
        {
            if (_close)
                return;
            if (_pointerIn)
                return;
            _pointerIn = true;
            ChangeColor(PointerInColorMod, 1);
        }
        public void Exit()
        {
            if (_close)
                return;
            if (!_pointerIn)
                return;
            _pointerIn = false;
            ChangeColor(PointerInColorMod, -1);
        }
    }
}