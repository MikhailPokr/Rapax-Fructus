using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    [RequireComponent(typeof(SphereCollider))]
    public class Projectile : MonoBehaviour
    {
        private GameObject _target;
        private int _damage;
        private float _speed;

        public void SetStats(GameObject target, int damage, float speed)
        {
            _damage = damage;
            _speed = speed;
            _target = target;
        }
        private void Update()
        {
            if (_target == null)
            {
                Destroy(gameObject);
                return;
            }
            transform.position = Vector3.LerpUnclamped(transform.position, _target.transform.position, _speed);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.ApplyDamage(_damage);
                Destroy(gameObject);
                return;
            }
        }
    }
}