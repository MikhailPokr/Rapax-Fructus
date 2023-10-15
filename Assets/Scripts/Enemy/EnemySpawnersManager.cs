using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Распоряжается спавном врагов на этаже.
    /// </summary>
    internal class EnemySpawnersManager : MonoBehaviour
    {
        /// <summary>
        /// Кулдаун спавна врагов.
        /// </summary>
        [SerializeField] private float _spawnTime;
        /// <summary>
        /// Входы на этаже.
        /// </summary>
        [SerializeField] private EnemySpawner[] _spawners;
        /// <summary>
        /// Входы на этаже.
        /// </summary>
        public EnemySpawner[] Spawners => _spawners;
        /// <summary>
        /// Словарь для баланса врагов на этаже. Чем выше разница с другими входами, тем меньше шанс, что на этом заспавниться враг.
        /// </summary>
        private readonly Dictionary<EnemySpawner, int> _spawnBalancer = new Dictionary<EnemySpawner, int>();
        /// <summary>
        /// Таймер.
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
        /// Обработчик события паузы. Останавливает спавн.
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
        /// Останавливает спавн.
        /// </summary>
        // Не уверен зачем отдельный метод.
        private void StopSpawn()
        {
            enabled = false;
        }

        /// <summary>
        /// Запускает спавн в первый раз. Вызывается из генератора.
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
        /// Спавнит противника там где нужно. 
        /// </summary>
        private void SpawnEnemy()
        {
            if (_spawners == null)
                return;
            Dictionary<EnemySpawner, float> chance = new Dictionary<EnemySpawner, float>();
            int max = _spawnBalancer.Values.Max();
            //не могу придумать норм название переменной для подобной системы.
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
        /// Ускоряет спавн.
        /// </summary>
        /// <param name="level"></param>
        public void SetSpawnTime(PowerUpObject powerUpObject)
        {
            _spawnTime = powerUpObject.GetPower("SpawnTime", _spawnTime);
        }
    }
    //нуждается в небольшой переработке для улучшения читаемости. Некоторые методы можно объединить.
}