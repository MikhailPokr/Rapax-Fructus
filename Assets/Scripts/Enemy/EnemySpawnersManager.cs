using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// ������������� ������� ������ �� �����.
    /// </summary>
    internal class EnemySpawnersManager : MonoBehaviour
    {
        /// <summary>
        /// ������� ������ ������.
        /// </summary>
        [SerializeField] private float _spawnTime;
        /// <summary>
        /// ����� �� �����.
        /// </summary>
        [SerializeField] private EnemySpawner[] _spawners;
        /// <summary>
        /// ����� �� �����.
        /// </summary>
        public EnemySpawner[] Spawners => _spawners;
        /// <summary>
        /// ������� ��� ������� ������ �� �����. ��� ���� ������� � ������� �������, ��� ������ ����, ��� �� ���� ������������ ����.
        /// </summary>
        private readonly Dictionary<EnemySpawner, int> _spawnBalancer = new Dictionary<EnemySpawner, int>();
        /// <summary>
        /// ������.
        /// </summary>
        private float _timer;

        private void Start()
        {
            if (ManagerDirectory.Instance == null)
            {
                enabled = false;
                return;
            }
            ManagerDirectory.Instance.TimeStop.TimeStopNotify += OnTimeStop;
            ManagerDirectory.Instance.CloseEntrances.CloseNotify += StopSpawn;
        }

        /// <summary>
        /// ���������� ������� �����. ������������� �����.
        /// </summary>
        /// <param name="stop"></param>
        public void OnTimeStop(bool stop)
        {
            if (ManagerDirectory.Instance.CloseEntrances.Close)
                return;
            if (stop)
                StopSpawn();
            else
                enabled = true;
        }

        /// <summary>
        /// ������������� �����.
        /// </summary>
        // �� ������ ����� ��������� �����.
        private void StopSpawn()
        {
            enabled = false;
        }

        /// <summary>
        /// ��������� ����� � ������ ���. ���������� �� ����������.
        /// </summary>
        public void StartSpawn()
        {
            foreach (EnemySpawner spawner in _spawners)
                _spawnBalancer.Add(spawner, 0);
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _spawnTime)
            {
                if (_spawnTime > 0.1f)
                    _spawnTime -= 0.01f;
                _timer = 0;
                SpawnEnemy();
            }
        }

        /// <summary>
        /// ������� ���������� ��� ��� �����. 
        /// </summary>
        private void SpawnEnemy()
        {
            if (_spawners == null)
                return;
            Dictionary<EnemySpawner, float> chance = new Dictionary<EnemySpawner, float>();
            int max = _spawnBalancer.Values.Max();
            //�� ���� ��������� ���� �������� ���������� ��� �������� �������.
            int division = 0;
            foreach (int count in _spawnBalancer.Values)
                division += max - count + 1;
            float previousChance = 0;
            foreach (var spawnData in _spawnBalancer)
            {
                float value = 1 / (float)division * (max - spawnData.Value + 1) + previousChance;
                chance.Add(spawnData.Key, value);
                previousChance = value;
            }
            float randomValue = Random.value;
            EnemySpawner spawner = chance.First(x => x.Value >= randomValue).Key;
            spawner.SpawnNewEnemy();
            _spawnBalancer[spawner]++;
            return;
        }

        /// <summary>
        /// �������� �����.
        /// </summary>
        /// <param name="level"></param>
        public void SetSpawnTime(PowerUpObject powerUpObject)
        {
            _spawnTime = powerUpObject.GetPower("SpawnTime", _spawnTime);
        }
    }
    //��������� � ��������� ����������� ��� ��������� ����������. ��������� ������ ����� ����������.
}