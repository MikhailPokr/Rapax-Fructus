using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

namespace RapaxFructus
{
    /// <summary>
    /// Содержит все необходимые списки для быстрого доступа к точкам.
    /// </summary>
    internal class PointsDirectory : MonoBehaviour
    {
        private EnemySpawnersManager _enemySpawnersManager;
        readonly private List<GroundPoint> _groundPoints = new();

        //ядро
        private CorePoint _corePoint;
        public CorePoint CorePoint => _corePoint;

        //камеры
        private Dictionary<int, Dictionary<CameraMovement.CameraPosition, CameraPoint>> _cameraPoints;
        public Dictionary<int, Dictionary<CameraMovement.CameraPosition, CameraPoint>> CameraPoints => _cameraPoints;

        //наземные
        public List<GroundPoint> GroundPoints => _groundPoints;

        readonly private Dictionary<SpawnPoint, GroundPoint> _StartGroundPoints = new();
        public Dictionary<SpawnPoint, GroundPoint> StartGroundPoints => _StartGroundPoints;

        private void OnEnable()
        {
            _enemySpawnersManager = gameObject.GetComponent<EnemySpawnersManager>();
        }

        public void UpdateDirectory()
        {
            List<CameraPoint> cameraPoints = GetComponentsInChildren<CameraPoint>().ToList();
            _cameraPoints = new();
            foreach (CameraPoint cameraPoint in cameraPoints)
            {
                if (!_cameraPoints.TryGetValue(cameraPoint.Line, out Dictionary<CameraMovement.CameraPosition, CameraPoint> line))
                {
                    _cameraPoints.Add(cameraPoint.Line, new Dictionary<CameraMovement.CameraPosition, CameraPoint>());
                }
                _cameraPoints[cameraPoint.Line].Add(cameraPoint.CameraPositionType, cameraPoint);
            }

            _corePoint = GetComponentInChildren<CorePoint>();

            List<SpawnPoint> spawnPoints = new();
            foreach (EnemySpawner spawner in _enemySpawnersManager.Spawners)
                spawnPoints.Add(spawner.GetComponentInChildren<SpawnPoint>());
            if (spawnPoints.Count == 0 || _groundPoints.Count == 0)
                return;
            foreach (SpawnPoint spawnPoint in spawnPoints)
            {
                int levelIndex = ManagerDirectory.Instance.LevelGenerator.Floors.IndexOf(spawnPoint.GetComponentInParent<LevelFloor>());
                _StartGroundPoints.Add
                    (spawnPoint,
                    _groundPoints.
                    First(x => Vector3.Distance(spawnPoint.transform.position, x.transform.position)
                    ==
                    _groundPoints.
                    Min(x => Vector3.Distance(spawnPoint.transform.position, x.transform.position))));
            }
        }
        public void AddGroundPoints(List<GroundPoint> groundPoints)
        {
            _groundPoints.AddRange(groundPoints);
        }
    }
}