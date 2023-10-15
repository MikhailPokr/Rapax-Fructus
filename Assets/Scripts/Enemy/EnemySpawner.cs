using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Скрипт, спавнящий указаных врагов на указанной точке.
    /// </summary>
    internal class EnemySpawner : MonoBehaviour
    {
        /// <summary>
        /// Враги, которых нужно заспавнить.
        /// </summary>
        [SerializeField] private Enemy[] _enemiesForSpawn;
        /// <summary>
        /// Точка спавна.
        /// </summary>
        [SerializeField] private SpawnPoint _spawnPoint;

        /// <summary>
        /// Список врагов еще не вышедших со спавна.
        /// </summary>
        private List<Enemy> _enemiesInZone = new List<Enemy>();

        public delegate void SpawnEnemyHandler(Enemy enemy);
        public event SpawnEnemyHandler SpawnEnemy;

        private void Update()
        {
            //изменение видимости врагов на выходе со спавна.
            for (int i = 0; i < _enemiesInZone.Count; i++)
            {
                float alpha = Vector3.Distance(_spawnPoint.transform.position, _enemiesInZone[i].transform.position) / _spawnPoint.Radius;
                if (alpha >= 1)
                {
                    ChangeVisibility(_enemiesInZone[i].gameObject, 1);
                    _enemiesInZone.Remove(_enemiesInZone[i]);
                    i--;
                }
                else
                {
                    ChangeVisibility(_enemiesInZone[i].gameObject, alpha);
                }
            }
        }

        /// <summary>
        /// Заспавнить нового врага.
        /// </summary>
        public void SpawnNewEnemy()
        {
            if (_enemiesForSpawn == null || _enemiesForSpawn.Length == 0)
                return;
            Enemy enemy = Instantiate(_enemiesForSpawn[Random.Range(0, _enemiesForSpawn.Length)], _spawnPoint.transform, false).GetComponent<Enemy>();
            ChangeVisibility(enemy.gameObject, 0);
            _enemiesInZone.Add(enemy);
            SpawnEnemy?.Invoke(enemy);
        }

        /// <summary>
        /// Измениить видимость заданного объекта на данную. Объект должен иметь соответвующий материал.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="alpha"></param>
        // можно кстати перенести в Utils или типа того.
        private void ChangeVisibility(GameObject gameObject, float alpha)
        {
            MeshRenderer[] meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                Color color = meshRenderer.material.color;
                meshRenderer.material.color = new Color(color.r, color.g, color.b, alpha);
            }
        }
    }
}