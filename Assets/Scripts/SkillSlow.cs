using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RapaxFructus
{
    internal class SkillSlow : Skill
    {
        private List<GroundEnemy> _groundEnemies = new();
        private void Start()
        {
            _groundEnemies = new List<GroundEnemy>();
            ManagerDirectory.Instance.Player.ResourseCountChanged += OnResourseCountChanged;
            foreach (LevelFloor levelFloor in ManagerDirectory.Instance.LevelGenerator.Floors)
            {
                foreach (EnemySpawner enemySpawner in levelFloor.EnemySpawnersManager.Spawners)
                {
                    enemySpawner.SpawnEnemy += OnSpawnEnemy;
                }
            }
        }
        private void OnSpawnEnemy(Enemy enemy)
        {
            GroundEnemy groundEnemy = enemy.GetComponent<GroundEnemy>();
            if (groundEnemy == null)
                return;
            _groundEnemies.Add(groundEnemy);
        }

        public override void Activate()
        {
            base.Activate();
            if (!_powerUpObject.Unlock)
                return;
            foreach (GroundEnemy enemy in _groundEnemies)
            {
                ManagerDirectory.Instance.Player.ChangeResourseCount(-_cost);
                StartCoroutine(enemy.ChangeSpeed(_powerUpObject));
            }
        }
    }
}