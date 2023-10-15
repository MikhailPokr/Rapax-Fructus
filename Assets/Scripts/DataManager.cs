using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace RapaxFructus
{
    public class DataManager : MonoBehaviour
    {
        private static SaveFile[] _saveFiles;
        private static int _currentSaveFileIndex;
        public static int CurrentSaveFileIndex => _currentSaveFileIndex;

        public delegate void PointsCountChangeHandler();
        public static event PointsCountChangeHandler PointsCountChange;
        private static int _energyPoints;
        public static int EnergyPoints => _energyPoints;
        public static int _skipOptions;
        public static int SkipOptions => _skipOptions;
        public static SaveFile Save => _saveFiles?[_currentSaveFileIndex];
        private static DataManager _instance;
        private void Start()
        {
            if (_instance == null)
                _instance = this;
            else
            {
                Destroy(_instance.gameObject);
                _instance = this;
            }
            if (SceneHelper.CurrentScene != SceneHelper.Scene.Menu)
                return;
            DontDestroyOnLoad(gameObject);
            
            _saveFiles = new SaveFile[SaveFileManager.Instance.SaveFiles.Length];
            for (int i = 0; i < _saveFiles.Length; i++)
            {
                _saveFiles[i] = Instantiate(SaveFileManager.Instance.SaveFiles[i], transform);
                _saveFiles[i].Copy(SaveFileManager.Instance.SaveFiles[i].CurrentGameData);
                _saveFiles[i].gameObject.SetActive(false);
            }
            _currentSaveFileIndex = SaveFileManager.Instance.CurrentSaveFileIndex;
            _energyPoints = PlayerPrefs.GetInt("EnergyPoints", 20);
            _skipOptions = PlayerPrefs.GetInt("SkipOptions", 3);
            SaveAll();
            PointsCountChange?.Invoke();
        }
        public static bool ChangeEnergyPoints(int delta, bool saveMode)
        {
            if (_energyPoints + delta < (saveMode ? 20 : 0))
                return false;
            _energyPoints += delta;
            PointsCountChange?.Invoke();
            PlayerPrefs.SetInt("EnergyPoints", _energyPoints);
            PlayerPrefs.Save();
            return true;
        }
        public static bool ChangeSkipOptions(int delta)
        {
            if (_skipOptions + delta < 0 || _skipOptions + delta > 3)
                return false;
            _skipOptions += delta;
            PlayerPrefs.SetInt("SkipOptions", _skipOptions);
            PlayerPrefs.Save();
            return true;
        }
        public static void SetIndex(int index)
        {
            _currentSaveFileIndex = index;
        }
        public static void SaveAll()
        {
            List<SaveFile.GameData> gameDatas = new();
            foreach (SaveFile file in _saveFiles)
            {
                gameDatas.Add(file.CurrentGameData);
            }
            Saver<SaveFile.GameData[]>.Save(SaveFileManager.FileName, gameDatas.ToArray());
        }
        public static void DeleteAll()
        {
            DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);
            foreach (FileInfo file in directory.GetFiles())
            {
                if (!file.Name.EndsWith(".dat"))
                    continue;
                File.Delete(file.FullName);
            }
        }
        private void OnDisable()
        {
            Destroy(gameObject);
        }
    }
}