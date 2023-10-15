using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace RapaxFructus
{
    internal class BulletLauncher : Wiapon
    {
        [SerializeField] private Projectile _projectile;
        protected override void OnEnable()
        {
            base.OnEnable();
            if (_cooldown == -1)
                throw new System.Exception("For the weapon \"BulletLauncher\" you need to set a rational cooldown");
        }
        protected override void Update()
        {
            if (_target == null)
                return;
            base.Update();
            if (_timer < _cooldown)
                return;
            _timer = 0;
            Projectile projectile = Instantiate(_projectile);
            projectile.transform.position = transform.position;
            projectile.SetStats(_target, _damage, _speed);
        }
        public override bool SetTarget(GameObject target)
        {
            _target = target;
            return true;
        }
    }
}