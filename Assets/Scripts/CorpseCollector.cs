using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RapaxFructus
{
    public class CorpseCollector : MonoBehaviour
    {
        [SerializeField] private float AbsorptionEfficiency;

        private const float _transparentSpeed = 0.01f;
        private const float _absorptionSpeed = 0.0005f;

        private List<MeshRenderer[]> _corpses = new List<MeshRenderer[]>();
        [SerializeField] private UnityEvent OnFloorCleared = new();
        private int _enemyCount = 0;

        private void Update()
        {
            if (_enemyCount == 0)
            {
                if (ManagerDirectory.Instance.LevelGenerator.Floors.FindAll(x => x != null && !x.PointsDirectory.CorePoint.Core.Dead).Count == 0)
                    enabled = false;
                if (ManagerDirectory.Instance.CloseEntrances.Close)
                {
                    OnFloorCleared?.Invoke();
                    enabled = false;
                }
                return;
            }
            for (int i = 0; i < _corpses.Count; i++)
            {
                if (_corpses[i][0] == null)
                    continue;
                foreach (MeshRenderer meshRenderer in _corpses[i])
                {
                    meshRenderer.material.color = new Color(meshRenderer.material.color.r, meshRenderer.material.color.g, meshRenderer.material.color.b, meshRenderer.material.color.a - _transparentSpeed);
                }  
                _corpses[i][0].transform.parent.position = new Vector3(_corpses[i][0].transform.parent.position.x, _corpses[i][0].transform.parent.position.y - _absorptionSpeed, _corpses[i][0].transform.parent.position.z);
                if (_corpses[i][0].material.color.a <= 0)
                {
                    _corpses[i][0].transform.parent.GetComponent<Enemy>().DeadEvent -= OnEnemyDie;
                    bool resourse = false;
                    if (_corpses[i][0].transform.parent.GetComponent<SpecialResourceEnemy>() != null &&
                        _corpses[i][0].transform.parent.GetComponent<SpecialResourceEnemy>().HasResourse)
                        ManagerDirectory.Instance.Player.ChangeResourseCount(1);
                    if (resourse ) { }
                    var corpse = _corpses[i][0].transform.parent.gameObject;
                    ManagerDirectory.Instance.Player.GiveEnergy(_corpses[i][0].transform.parent.GetComponent<Enemy>().MaxHP * AbsorptionEfficiency);
                    _corpses.Remove(_corpses[i]);
                    Destroy(corpse);
                    _enemyCount--;

                    i = i == 0 ? 0 : i - 1;
                }
            }
        }
        public void SetSpawnNotify()
        {
            foreach(LevelFloor levelFloor in ManagerDirectory.Instance.LevelGenerator.Floors)
            {
                foreach(EnemySpawner enemySpawner in levelFloor.EnemySpawnersManager.Spawners)
                {
                    enemySpawner.SpawnEnemy += OnEnemySpawn;
                }
                
            }
        }
        private void OnEnemySpawn(Enemy enemy)
        {
            enemy.DeadEvent += OnEnemyDie; 
            _enemyCount++;
        }
        private void OnEnemyDie(Enemy enemy, bool noEnergy)
        {
            if (noEnergy)
            {
                enemy.DeadEvent -= OnEnemyDie;
                _enemyCount--;
                return;
            }  
            _corpses.Add(enemy.GetComponentsInChildren<MeshRenderer>());
        }
    }
}