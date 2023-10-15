using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Этаж фрукта(уровня).
    /// </summary>
    [RequireComponent(typeof(TilesGenerator))]
    [RequireComponent(typeof(EnemySpawnersManager))]
    [RequireComponent(typeof(PointsDirectory))]
    [Serializable]
    internal class LevelFloor : MonoBehaviour
    {
        public int EnergyCost;
        public int MaxEnergy;

        private TilesGenerator _tilesGenerator;
        public TilesGenerator TileGenerator => _tilesGenerator;

        private PointsDirectory _pointsDirectory;
        public PointsDirectory PointsDirectory => _pointsDirectory;

        private EnemySpawnersManager _spawnersManager;
        public EnemySpawnersManager EnemySpawnersManager => _spawnersManager;

        [SerializeField] private PowerUpObject _powerUpObject;
        public PowerUpObject PowerUpObject => _powerUpObject;

        private PartType _size = PartType.Adapter;
        public PartType Size => _size;
        private PartType _entrance = PartType.Adapter;
        public PartType Entrance => _entrance;

        public void Set(PartType size, PartType entrance)
        {
            if (_size != PartType.Adapter && _entrance != PartType.Adapter)
                return;
            _size = size;
            _entrance = entrance;
        }

        private void OnEnable()
        {
            _tilesGenerator = gameObject.GetComponent<TilesGenerator>();
            _pointsDirectory = gameObject.GetComponent<PointsDirectory>();
            _spawnersManager = gameObject.GetComponent<EnemySpawnersManager>();
        }
        public void SetDestroyNotify()
        {
            _pointsDirectory.CorePoint.Core.DeadNotify += DestroyFloor;
        }
        public void DestroyFloor()
        {
            List<MonoBehaviour> gameObjects = new();
            gameObjects.AddRange(gameObject.GetComponentsInChildren<CameraPoint>());
            gameObjects.AddRange(gameObject.GetComponentsInChildren<Enemy>());
            gameObjects.AddRange(gameObject.GetComponentsInChildren<Obstacle>());
            gameObjects.AddRange(gameObject.GetComponentsInChildren<EnemySpawner>());
            foreach (var gObject in gameObjects)
            {
                Destroy(gObject.gameObject);
            }
            gameObject.GetComponent<EnemySpawnersManager>().enabled = false;
            if (ManagerDirectory.Instance.CameraMovement.CurrentLevel - 1 == ManagerDirectory.Instance.LevelGenerator.Floors.FindIndex(x => x == this))
                ManagerDirectory.Instance.CameraMovement.ManualAction(ControlSystem.Action.MoveUp);
            ManagerDirectory.Instance.LevelGenerator.OnLevelDestroy(this);
        }
    }
}