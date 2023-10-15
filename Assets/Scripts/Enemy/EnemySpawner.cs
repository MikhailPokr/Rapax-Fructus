using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// ������, ��������� �������� ������ �� ��������� �����.
    /// </summary>
    internal class EnemySpawner : MonoBehaviour
    {
        /// <summary>
        /// �����, ������� ����� ����������.
        /// </summary>
        [SerializeField] private Enemy[] _enemiesForSpawn;
        /// <summary>
        /// ����� ������.
        /// </summary>
        [SerializeField] private SpawnPoint _spawnPoint;

        /// <summary>
        /// ������ ������ ��� �� �������� �� ������.
        /// </summary>
        private List<Enemy> _enemiesInZone = new List<Enemy>();

        public delegate void SpawnEnemyHandler(Enemy enemy);
        public event SpawnEnemyHandler SpawnEnemy;

        private void Update()
        {
            //��������� ��������� ������ �� ������ �� ������.
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
        /// ���������� ������ �����.
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
        /// ��������� ��������� ��������� ������� �� ������. ������ ������ ����� ������������� ��������.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="alpha"></param>
        // ����� ������ ��������� � Utils ��� ���� ����.
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