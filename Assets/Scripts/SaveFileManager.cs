using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RapaxFructus
{
    public class SaveFileManager : SingletonBase<SaveFileManager>
    {
        public const string FileName = "saveFiles.dat";
        [SerializeField] private SaveFile[] _saveFiles;
        public SaveFile[] SaveFiles => _saveFiles;

        private int _currentSaveFileIndex = 0;
        public int CurrentSaveFileIndex => _currentSaveFileIndex;
        public CameraPoint CurrentCameraPoint => _saveFiles[_currentSaveFileIndex].CameraPoint;
        public SaveFile CurrentSaveFile => _saveFiles[_currentSaveFileIndex];
        [SerializeField] private PowerUpList _powerUpList;
        override protected void Awake()
        {
            base.Awake();
            ControlSystem.Instance.ActionNotify += OnAction;

            PowerUpList.SetInstance(_powerUpList);
            if (!Saver<SaveFile.GameData[]>.TryLoad(FileName, out SaveFile.GameData[] file) || file == null)
            {
                List<SaveFile.GameData> gameDatas = new();
                foreach (var save in _saveFiles)
                {
                    save.CurrentGameData.SetDefaultOrNothing();
                    gameDatas.Add(save.CurrentGameData);
                }
                Saver<SaveFile.GameData[]>.Save(FileName, gameDatas.ToArray());
            }
            else
            {
                for (int i = 0; i < _saveFiles.Length; i++)
                {
                    _saveFiles[i].Copy(file[i]);
                }
            }
            for (int i = 0; i < _saveFiles.Length; i++)
            {
                _saveFiles[i].GenerateFruit();
            }
            CameraMovement.Instance.ManualAction(0);
        }

        public void OnAction(ControlSystem.Action action)
        {
            if (action != ControlSystem.Action.MoveRight && action != ControlSystem.Action.MoveLeft)
                return;
            if (CameraMovement.Instance.Lock)
                return;
            switch (action)
            {
                case ControlSystem.Action.MoveRight:
                    {
                        _currentSaveFileIndex++;
                        if (_currentSaveFileIndex >= _saveFiles.Length)
                            _currentSaveFileIndex = 0;
                        break;
                    }
                case ControlSystem.Action.MoveLeft:
                    {
                        _currentSaveFileIndex--;
                        if (_currentSaveFileIndex <= -1)
                            _currentSaveFileIndex = _saveFiles.Length - 1;
                        break;
                    }
            }
            CameraMovement.Instance.ManualAction(0);
            DataManager.SetIndex(_currentSaveFileIndex);
        }
    }
}