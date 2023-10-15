using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    internal class BuildManager : MonoBehaviour
    {
        private int _num;
        private int _lastNum;
        public bool NoTurret => _num < 0;

        [SerializeField] private Color _color;
        public Color Color => _color;

        [SerializeField] private List<Obstacle> _tinyObstacles;
        public delegate void ObstacleChangedHandler();
        public event ObstacleChangedHandler ObstacleChanged;

        private void Start()
        {
            ManagerDirectory.Instance.ControlSystem.ActionNotify += OnAction;
        }

        private void OnAction(ControlSystem.Action action)
        {
            if (action == ControlSystem.Action.BuildMode)
            {
                if (_num == -1)
                    SetTurret(_lastNum);
                else
                {
                    _lastNum = _num;
                    SetTurret(-1);
                }
            }
        }

        public Obstacle Get(PartType partType)
        {
            if (_num == -1)
                return null;
            switch (partType)
            {
                case PartType.Tiny:
                    {
                        if (_num < _tinyObstacles.Count)
                            return _tinyObstacles[_num];
                        break;
                    }
            }
            return null;
        }

        public void SetTurret(int num)
        {
            _num = num;
            ObstacleChanged.Invoke();
        }
    }
}