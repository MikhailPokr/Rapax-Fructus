using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapaxFructus
{

    [Serializable]
    public class SaveFile : MonoBehaviour
    {
        [SerializeField] private CameraPoint _cameraPoint;
        
        public CameraPoint CameraPoint => _cameraPoint;
        public GameData CurrentGameData;
        
        public void GenerateFruit()
        {
            LevelGenerator levelGenerator = GetComponent<LevelGenerator>();
            levelGenerator.SetMaps(CurrentGameData.LevelMap, CurrentGameData.EntranceMap);
        }

        public void Copy(GameData gameData)
        {
            CurrentGameData = gameData;
            gameData.SetDefaultOrNothing();
        }


        [Serializable]
        public class GameData
        {
            public List<PartType> LevelMap;
            public List<PartType> EntranceMap;
            public List<string> PowerUpsKeys; // Список ключей словаря PowerUps
            public List<int> PowerUpsValues; // Список значений словаря PowerUps
            public List<string> Unlocks;
            private Dictionary<string, int> _powerUps;
            public Dictionary<string, int> PowerUps
            {
                get
                {
                    if (_powerUps == null || _powerUps.Count == 0)
                    {
                        var powerUps = new Dictionary<string, int>();
                        for (int i = 0; i < PowerUpsKeys.Count; i++)
                        {
                            powerUps[PowerUpsKeys[i]] = PowerUpsValues[i];
                        }
                        _powerUps = powerUps;
                    }
                    return _powerUps;
                }
                set
                {
                    PowerUpsKeys = new List<string>(value.Keys);
                    PowerUpsValues = new List<int>(value.Values);
                    _powerUps.Clear();
                }
            }

            public void SetDefaultOrNothing()
            {
                LevelMap ??= new List<PartType>();
                EntranceMap ??= new List<PartType>();
                PowerUpsKeys ??= new List<string>();
                PowerUpsValues ??= new List<int>();
                Unlocks ??= new List<string>();

                foreach (PowerUpObject element in PowerUpList.Instance.PowerUpElements)
                {
                    for (int i = 0; i < element.PowerUps.Length; i++)
                    {
                        if (!PowerUpsKeys.Contains(element.Name + element.PowerUps[i].Name))
                        {
                            PowerUpsKeys.Add(element.Name + element.PowerUps[i].Name);
                            PowerUpsValues.Add(0);
                        }
                    }
                }
            }

            public void Clear()
            {
                LevelMap.Clear();
                EntranceMap.Clear();
                PowerUpsKeys.Clear();
                PowerUpsValues.Clear();
                Unlocks.Clear();
            }

            public void AddLevel(string name)
            {
                int i = PowerUpsKeys.FindIndex(x => x == name);
                PowerUpsValues[i] += 1;
                _powerUps.Clear();
            }

            public void UnlockOrLock(string name, bool unlock)
            {
                if (unlock)
                    Unlocks.Add(name);
                else
                    Unlocks.Remove(name);
            }
        }


    }
}