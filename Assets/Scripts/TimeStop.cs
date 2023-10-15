using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    public class TimeStop : MonoBehaviour
    {
        private bool _stop = false;
        public bool Stop => _stop;
        public delegate void TimeStopEvent(bool stop);
        
        public event TimeStopEvent TimeStopNotify;

        private void OnEnable()
        {
            ControlSystem.Instance.ActionNotify += OnAction;
        }

        private void OnAction(ControlSystem.Action action)
        {
            if (action == ControlSystem.Action.TimeStop)
            {
                Click();
            }
        }

        public void Click()
        {
            _stop = !_stop;
            TimeStopNotify?.Invoke(_stop);
        }
    }
}