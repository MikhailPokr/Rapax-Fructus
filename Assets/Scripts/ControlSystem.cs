using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Реализует обработку физического ввода.
    /// </summary>
    public class ControlSystem : SingletonBase<ControlSystem>
    {
        public delegate void ActionEvent(Action action);
        public event ActionEvent ActionNotify;

        public enum Action
        {
            _,
            Pause,
            TimeStop,
            MoveRight,
            MoveLeft,
            MoveUp,
            MoveDown,
            MoveIn,
            MoveOut,
            BuildMode
        }

        private Dictionary<KeyCode, Action> _actions = new Dictionary<KeyCode, Action>()
        {
            {KeyCode.Escape, Action.Pause},
            {KeyCode.Space, Action.TimeStop},
            {KeyCode.D, Action.MoveRight},
            {KeyCode.A, Action.MoveLeft},
            {KeyCode.W, Action.MoveUp},
            {KeyCode.S, Action.MoveDown},
            {KeyCode.UpArrow, Action.MoveIn},
            {KeyCode.DownArrow, Action.MoveOut},
            {KeyCode.Tab, Action.BuildMode},
        };
        public void ManualAction(Action action)
        {
            if (action != Action._)
                ActionNotify?.Invoke(action);
        }

        private void Update()
        {
            Action action = Action._;
            foreach (KeyCode key in _actions.Keys)
            {
                if (Input.GetKeyDown(key))
                {
                    action = _actions[key];
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    action = Action.MoveIn;
                }
                else
                {
                    action = Action.MoveOut;
                }
            }
            if (action != Action._)
                ActionNotify?.Invoke(action);
        }
        
    }
}