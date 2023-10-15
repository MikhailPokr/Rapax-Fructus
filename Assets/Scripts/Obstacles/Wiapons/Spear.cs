using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RapaxFructus
{
    internal class Spear : Wiapon
    {
        [SerializeField] private GameObject _shaft;
        [SerializeField] private GameObject _head;
        [SerializeField] private BoxCollider _collider;

        private Quaternion _shaftStartRotation;
        private Quaternion _headStartRotation;
        
        public bool Active => _target != null;

        private bool _onTarget = true;

        protected override void OnEnable()
        {
            base.OnEnable();
            _collider.size = _head.transform.localScale;
            _shaftStartRotation = _shaft.transform.rotation;
            _headStartRotation = _head.transform.rotation;
        }

        public override bool SetTarget(GameObject target)
        {
            if (Active)
                return false;
            _target = target;
            target = Instantiate(new GameObject("Point"));
            target.transform.position = _target.transform.position;
            _target = target;
            Vector3 relativePos = - target.transform.position + transform.position;
            _head.transform.rotation = Quaternion.FromToRotation(_headStartRotation.eulerAngles, relativePos);
            _shaft.transform.rotation = Quaternion.FromToRotation(_shaft.transform.up, relativePos);
            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Enemy enemy))
            {
                enemy.ApplyDamage(_damage);
            }
        }

        protected override void Update()
        {
            if (!Active)
                return;
            
            if (_onTarget)
            {
                _head.transform.position = Vector3.MoveTowards(_head.transform.position, _target.transform.position, _speed);
                
                if (_head.transform.position == _target.transform.position)
                    _onTarget = false;
            }
            else 
            {
                _head.transform.position = Vector3.MoveTowards(_head.transform.position, transform.position, _speed * 2);

                if (_head.transform.position == transform.position)
                {
                    Destroy(_target);
                    _target = null;
                    _onTarget = true;
                    _shaft.transform.rotation = _shaftStartRotation;
                    _head.transform.rotation = _headStartRotation;
                }
            }
            _shaft.transform.localScale = new Vector3(_shaft.transform.localScale.x, Vector3.Distance(transform.position, _head.transform.position) / 2, _shaft.transform.localScale.z);
            _shaft.transform.position = transform.position + (-transform.position + _head.transform.position) / 2;
            _collider.center = _head.transform.localPosition;
        }
    }
}