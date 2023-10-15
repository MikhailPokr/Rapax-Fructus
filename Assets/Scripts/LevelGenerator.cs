using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RapaxFructus
{
    
    /// <summary>
    /// —оздает уровень, использу€ префабы из LevelPartsDirectory.
    /// </summary>
    internal class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private bool _generateOnAwake;
        [SerializeField] private AnchorPoint _topPoint;
        [SerializeField] private GameObject _parentLevel;
        [SerializeField] private List<PartType> _levelMap;
        [SerializeField] private List<PartType> _levelMapEntrance;
        [Space]
        [SerializeField] private bool _dontGenerateTiles;
        public bool DontGenerateTiles => _dontGenerateTiles;
        [Space]
        [SerializeField] private LevelPartsDirectory _levelPartsDirectory;
        [SerializeField] private CorpseCollector _corpseCollector;
        private Dictionary<string, int> _powerUps;
        readonly private List<LevelFloor> _floors = new();
        public List<LevelFloor> Floors => _floors;
        [SerializeField] private UnityEvent OnGameEnd;
        private bool _gameEnd = false;
        public bool GameEnd => _gameEnd;
        private int _cost;
        public int Cost => _cost;

        public void Awake()
        {
            if (_generateOnAwake)
            {
                if (DataManager.Save == null)
                {
                    Generate();
                    return;
                }
                List<PartType> map = DataManager.Save.CurrentGameData.LevelMap.ToList().ConvertAll(x => (PartType)x);
                List<PartType> entrance = DataManager.Save.CurrentGameData.EntranceMap.ToList().ConvertAll(x => (PartType)x);
                if (map.Count == 0)
                {
                    map.Add(PartType.Tiny);
                    entrance.Add(PartType.Tiny);
                }
                SetPowerUps(DataManager.Save.CurrentGameData.PowerUps);
                SetMaps(map, entrance);
                _cost = GetNecessaryEnergy();
            }
        }

        public void Generate()
        {
            if (_levelMap == null || _levelMap.Count == 0)
            {
                GameObject nonFruit = Instantiate(_levelPartsDirectory.NonFruit, _parentLevel.transform);
                AnchorPoint point = GetComponentInChildren<AnchorPoint>();
                Vector3 delta0 = -point.transform.position + _topPoint.transform.position;
                nonFruit.transform.position += delta0;
                return;
            }
            GameObject leaves = Instantiate(_levelPartsDirectory.Leaves, _parentLevel.transform, false);
            AnchorPoint[] points = leaves.GetComponentsInChildren<AnchorPoint>();
            AnchorPoint secondpoint = points[0].transform.position.y > points[1].transform.position.y ? points[0] : points[1];
            Vector3 delta = -secondpoint.transform.position + _topPoint.transform.position;
            leaves.transform.position += delta;
            AnchorPoint firstPoint = points[0].transform.position.y < points[1].transform.position.y ? points[0] : points[1];

            GameObject top = Instantiate(_levelPartsDirectory.Parts[_levelMap[0]]["Top"], _parentLevel.transform, false);
            points = top.GetComponentsInChildren<AnchorPoint>();
            secondpoint = points[0].transform.position.y > points[1].transform.position.y ? points[0] : points[1];
            delta = -secondpoint.transform.position + firstPoint.transform.position;
            top.transform.position += delta;
            firstPoint = points[0].transform.position.y < points[1].transform.position.y ? points[0] : points[1];
            for (int i = 0; i < _levelMap.Count; i++)
            {
                GameObject floor = Instantiate(_levelPartsDirectory.Parts[_levelMap[i]]["Level" + Enum.GetName(typeof(PartType), _levelMapEntrance[i])], _parentLevel.transform, false);
                points = floor.GetComponentsInChildren<AnchorPoint>();
                if (points.Length != 2)
                    throw new Exception($"Level generation error. Anchors at the {Enum.GetName(typeof(PartType), _levelMap[i])} Level part: {points.Length}");
                secondpoint = points[0].transform.position.y > points[1].transform.position.y ? points[0] : points[1];
                delta = -secondpoint.transform.position + firstPoint.transform.position;
                floor.transform.position += delta;
                firstPoint = points[0].transform.position.y < points[1].transform.position.y ? points[0] : points[1];
                LevelFloor levelFloor = floor.GetComponent<LevelFloor>();
                _floors.Add(levelFloor);
                levelFloor.Set(_levelMap[i], _levelMapEntrance[i]);
                if (i != _levelMap.Count - 1)
                {
                    bool rotate = false;
                    string name = $"{Enum.GetName(typeof(PartType), _levelMap[i])}To{Enum.GetName(typeof(PartType), _levelMap[i + 1])}";
                    if (!_levelPartsDirectory.Parts[PartType.Adapter].ContainsKey(name))
                    {
                        rotate = true;
                        name = $"{Enum.GetName(typeof(PartType), _levelMap[i + 1])}To{Enum.GetName(typeof(PartType), _levelMap[i])}";
                    }
                    GameObject adapter = Instantiate(_levelPartsDirectory.Parts[PartType.Adapter][name], _parentLevel.transform, false);
                    if (rotate)
                        adapter.transform.rotation = Quaternion.Euler(180, 0, 0);
                    points = adapter.GetComponentsInChildren<AnchorPoint>();
                    if (points.Length != 2)
                        throw new Exception($"Level generation error. Anchors at the {name} adapter part: {points.Length}");
                    secondpoint = points[0].transform.position.y > points[1].transform.position.y ? points[0] : points[1];
                    delta = -secondpoint.transform.position + firstPoint.transform.position;
                    adapter.transform.position += delta;
                    firstPoint = points[0].transform.position.y < points[1].transform.position.y ? points[0] : points[1];
                }
            }
            GameObject bottom = Instantiate(_levelPartsDirectory.Parts[_levelMap[^1]]["Bottom"], _parentLevel.transform, false);
            secondpoint = bottom.GetComponentInChildren<AnchorPoint>();
            delta = -secondpoint.transform.position + firstPoint.transform.position;
            bottom.transform.position += delta;

            if (_dontGenerateTiles)
                return;
            GetComponent<Player>().SetPowerUpList(_powerUps);
            List<Core> cores = new();
            foreach (LevelFloor floor in _floors)
            {
                floor.TileGenerator.Generate();
                floor.PointsDirectory.UpdateDirectory();
                floor.EnemySpawnersManager.StartSpawn();
                cores.Add(floor.PointsDirectory.CorePoint.Core);
                floor.SetDestroyNotify();
            }
            ManagerDirectory.Instance.Player.Activate(cores);
            ManagerDirectory.Instance.CoreHpsDisplayManager.Activate();
            ManagerDirectory.Instance.EnengyDisplay.Activate();
            _corpseCollector.SetSpawnNotify();
            ManagerDirectory.Instance.CameraMovement.ChangePoint();

            if (DataManager.Save == null)
                return;
            List<PartType> map = new();
            List<PartType> entrance = new();
            foreach (LevelFloor levelFloor in _floors)
            {
                map.Add(levelFloor.Size);
                entrance.Add(levelFloor.Entrance);
                levelFloor.PointsDirectory.CorePoint.Core.SetTurret();
                levelFloor.PointsDirectory.CorePoint.Core.SetHp(levelFloor.PowerUpObject);
                levelFloor.GetComponent<EnemySpawnersManager>().SetSpawnTime(levelFloor.PowerUpObject);
                
            }
            DataManager.Save.CurrentGameData.LevelMap = map;
            DataManager.Save.CurrentGameData.EntranceMap = entrance;
            DataManager.SaveAll();
        }

        public void SetMaps(List<PartType> levelMap, List<PartType> levalMapEntrance)
        {
            _levelMap = levelMap;
            _levelMapEntrance = levalMapEntrance;
            Transform[] transforms = _parentLevel.GetComponentsInChildren<Transform>();
            if (!_generateOnAwake)
            {
                foreach (Transform t in transforms)
                {
                    if (t != transform && !t.TryGetComponent(out CameraPoint cameraPoint))
                            Destroy(t.gameObject);
                }
            }
            Generate();
        }
        public void SetPowerUps(Dictionary<string, int> list)
        {
            if (_dontGenerateTiles)
                return;
            _powerUps = list;
        }
        public void OnLevelDestroy(LevelFloor levelFloor)
        {
            _floors[_floors.FindIndex(x => x == levelFloor)] = null;
            if (_floors.Count(x => x == null) == _floors.Count)
            {
                OnGameEnd?.Invoke();
                _corpseCollector.enabled = false;
                _gameEnd = true;
            }
        }
        public int GetNecessaryEnergy()
        {
            int result = 20;
            foreach (LevelFloor floor in Floors)
            {
                result += floor.EnergyCost;
            }
            return result;
        }
        public int GetMaxEnergy()
        {
            int result = 0; 
            foreach (LevelFloor floor in Floors)
            {
                result += floor.MaxEnergy;
            }
            return result;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_levelMap == null && _levelMapEntrance == null)
                return;
            if (_levelMap.Count > _levelMapEntrance.Count)
            {
                _levelMapEntrance.Add(PartType.Tiny);
            }
            else if (_levelMap.Count < _levelMapEntrance.Count)
            {
                _levelMapEntrance.RemoveAt(_levelMapEntrance.Count - 1);
            }
            for (int i = 0; i < _levelMap.Count; i++)
            {
                if (_levelMap[i] == PartType.Adapter)
                {
                    _levelMap[i] = PartType.Tiny;
                    Debug.LogWarning("This is the technical part of the level, you don't need to use it.");
                }
                if (_levelMapEntrance[i] == PartType.Adapter)
                {
                    _levelMapEntrance[i] = PartType.Tiny;
                    Debug.LogWarning("This is the technical part of the level, you don't need to use it.");
                }
                if (!_levelPartsDirectory.Parts[_levelMap[i]].ContainsKey("Level" + Enum.GetName(typeof(PartType), _levelMapEntrance[i])))
                {
                    _levelMapEntrance[i] = PartType.Tiny;
                    Debug.LogWarning("This level does not have passages of this type");
                }
            }
        }
#endif
    }
}