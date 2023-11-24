using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// ѕоворачивает камеру, принима€ сигналы от ControlSystem.
    /// </summary>
    public class CameraMovement : SingletonBase<CameraMovement>
    {   
        public enum CameraPosition
        {
            Fruit,
            Level,
            Line
        }

        [SerializeField] private float MovementSpeed;
        [SerializeField] private float RotationSpeed;
        [SerializeField] private float TransparentSpeed;
        [SerializeField] private bool _wiggle;
        [SerializeField] private bool _isWiggling = false; 
        [SerializeField] private float _wiggleIntensity;
        [SerializeField] private float _wiggleDuration; 
        private float _wiggleTimer = 0f;

        public Material Transparent;

        private int _currentLevel;
        private int _currentLine;
        private CameraPosition _position;

        public int CurrentLevel => _currentLevel;
        public int Line => _currentLine;
        public CameraPosition Position => _position;

        private CameraPoint _target;
        public delegate void PositionChangedEvent();
        public event PositionChangedEvent PositionChanged;

        private ControlSystem.Action _lastAction;
        private bool _lock = false;
        public bool Lock => _lock;

        protected override void Awake()
        {
            base.Awake();
            if (ManagerDirectory.Instance != null)
                ManagerDirectory.Instance.ControlSystem.ActionNotify += OnActionHappened;
            else
                ControlSystem.Instance.ActionNotify += OnActionHappened;

            _currentLevel = 1;
            _currentLine = 1;
            _position = CameraPosition.Fruit;
        }

        private void Update()
        {
            if (_target == null)
                return;

            if ((_wiggle && Vector3.Distance(Camera.main.gameObject.transform.position, _target.transform.position) > MovementSpeed) ||
                (!_wiggle && ((Vector3.Distance(Camera.main.gameObject.transform.position, _target.transform.position) > MovementSpeed)
                || (Quaternion.Angle(Camera.main.gameObject.transform.rotation, _target.transform.rotation) > RotationSpeed))))
            {
                _isWiggling = false;
                Camera.main.gameObject.transform.SetPositionAndRotation
                    (Vector3.Lerp(Camera.main.gameObject.transform.position, _target.transform.position, MovementSpeed * Time.deltaTime * 100),
                    Quaternion.Lerp(Camera.main.gameObject.transform.rotation, _target.transform.rotation, RotationSpeed * Time.deltaTime * 100));
                Transparent.color = new Color(Transparent.color.r, Transparent.color.g, Transparent.color.b, Mathf.Lerp(Transparent.color.a, _target.Transparent, TransparentSpeed));
            }
            else
            {
                if (_wiggle && !_isWiggling)
                {
                    _isWiggling = true;
                    _wiggleTimer = 0f;
                }
            }

            if (_isWiggling)
            {
                _wiggleTimer += Time.deltaTime;

                if (_wiggleTimer <= _wiggleDuration)
                {
                    float angle = Mathf.Sin(_wiggleTimer * Mathf.PI * 2f / _wiggleDuration) * _wiggleIntensity;

                    Camera.main.gameObject.transform.rotation = _target.transform.rotation * Quaternion.Euler(0f, angle, 0f);
                }
                else
                {
                    _isWiggling = false;
                    _wiggleTimer = 0f;
                    Camera.main.gameObject.transform.rotation = _target.transform.rotation;
                }
            }
        }

        public void ManualAction(int modificator)
        {
            ControlSystem.Instance.ManualAction(modificator == 1 ? ControlSystem.Action.MoveRight : modificator == -1 ? ControlSystem.Action.MoveLeft : ControlSystem.Action._);
            if (modificator == 0)
                OnActionHappened(ControlSystem.Action._);
        }
        public void ManualAction(ControlSystem.Action action)
        {
            OnActionHappened(action);
        }
        private void OnActionHappened(ControlSystem.Action action)
        {
            _lastAction = action;
            if (!new List<ControlSystem.Action>
            { 
                ControlSystem.Action.MoveRight,
                ControlSystem.Action.MoveLeft,
                ControlSystem.Action.MoveUp,
                ControlSystem.Action.MoveDown,
                ControlSystem.Action.MoveIn,
                ControlSystem.Action.MoveOut,
                ControlSystem.Action._
            }.Contains(action))
                return;
            if (_lock)
                return;
            if (SceneHelper.CurrentScene == SceneHelper.Scene.Menu)
            {                
                ChangePoint(SaveFileManager.Instance.CurrentCameraPoint);
            }
            else if (SceneHelper.CurrentScene == SceneHelper.Scene.Game)
            {
                if (action == ControlSystem.Action._)
                    return;
                if (ManagerDirectory.Instance.LevelGenerator.GameEnd)
                    return;
                switch (action)
                {
                    case ControlSystem.Action.MoveRight:
                        _currentLine -= 1;
                        if (_currentLine < 1)
                            _currentLine = 4;
                        break;
                    case ControlSystem.Action.MoveLeft:
                        _currentLine += 1;
                        if (_currentLine > 4)
                            _currentLine = 1;
                        break;
                    case ControlSystem.Action.MoveUp:
                        _currentLevel -= 1;
                        if (_currentLevel < 1)
                            _currentLevel = ManagerDirectory.Instance.LevelGenerator.Floors.Count;
                        break;
                    case ControlSystem.Action.MoveDown:
                        _currentLevel += 1;
                        if (_currentLevel > ManagerDirectory.Instance.LevelGenerator.Floors.Count)
                            _currentLevel = 1;
                        break;
                    case ControlSystem.Action.MoveIn:
                        if ((int)_position + 1 > 2)
                            return;
                        _position = (CameraPosition)((int)_position + 1);
                        break;
                    case ControlSystem.Action.MoveOut:
                        if ((int)_position - 1 < 0)
                            return;
                        _position = (CameraPosition)((int)_position - 1);
                        break;
                }
                ChangePoint();
            }
        }
        public void ChangeLevel(int level)
        {
            _currentLevel = level;
        }
        public void ChangePoint()
        {
            if (ManagerDirectory.Instance.LevelGenerator.Floors[_currentLevel - 1] == null)
            { 
                OnActionHappened(_lastAction);
                return;
            }
            _target = ManagerDirectory.Instance.LevelGenerator.Floors[_currentLevel - 1].PointsDirectory.CameraPoints[_currentLine][_position];
            PositionChanged?.Invoke();
        }
        public void ChangePoint(CameraPoint newPoint)
        {
            _target = newPoint;
            PositionChanged?.Invoke();
        }
        public void SwitchLock()
        {
            _lock = !_lock;
        }
    }
}