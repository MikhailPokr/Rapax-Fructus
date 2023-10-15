using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Класс турели, которая имеет лист с вооружением, который она ипользует.
    /// </summary>
    internal class Turret : Obstacle
    {
        [SerializeField] private float _updateTime;
        [SerializeField] private Collider[] _clickColliders;
        private float _timer;

        private List<Wiapon> _wiaponsList;

        [HideInInspector] public TurretPoint MyTurretPoint;

        private void OnEnable()
        {
            if (!_alreadyEnabled)
                _alreadyEnabled = true;
            else
                return;
            _wiaponsList = GetComponentsInChildren<Wiapon>().ToList();
            _wiaponsList.OrderBy(x => x.Range);
        }

        private void Update()
        {
            if (_wiaponsList.Count == 0)
                return;
            _timer += Time.deltaTime;
            if (_timer < _updateTime)
                return;
            _timer = 0;
            if (!ManagerDirectory.Instance.Player.UseEnergy(_wiaponsList.Sum(x => x.EnergyConsumption)))
            {
                Destroy();
            }    
            Collider[] objectsColliders = Physics.OverlapSphere(transform.position, _wiaponsList[0].Range);
            List<Enemy> enemies = new();
            foreach (Collider objectCollider in objectsColliders)
            {
                if (objectCollider.TryGetComponent(out Enemy enemy) && !enemy.Dead)
                    enemies.Add(enemy);
            }
            if (enemies.Count == 0)
                return;
            enemies.OrderBy(x => Vector3.Distance(x.transform.position, x.DirectoryOfPoints.CorePoint.transform.position));
            int i = 0;
            foreach(Wiapon wiapon in _wiaponsList)
            {
                if (enemies.Count <= i)
                    i--;
                if (!wiapon.SetTarget(enemies[i].gameObject))
                    continue;
                i++;
            }
        }
        public void Destroy()
        {
            MyTurretPoint.Enable();
            Destroy(gameObject);
        }
        public void SetClickable(bool enabled)
        {
            foreach (Collider collider in _clickColliders)
                collider.enabled = enabled;
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach(Wiapon wiapon in _wiaponsList)
            {
                Gizmos.color = new Color(0.2f, 0.5f, 0.2f, 0.1f);
                Gizmos.DrawSphere(wiapon.transform.position, wiapon.Range);
            }
        }
#endif
    }
}