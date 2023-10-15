using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    internal class Core : MonoBehaviour
    {
        public LevelFloor Floor => GetComponentInParent<LevelFloor>(); 

        [SerializeField] private int _maxHP;
        [SerializeField] private Turret _turret;
        public int MaxHP => _maxHP;
        private int _currentHP;
        public int CurrentHP => _currentHP;
        public bool Dead => _currentHP <= 0;

        public delegate void HPChangedEvent(Core core);
        public event HPChangedEvent HPChanged;

        public delegate void DeadEvent();
        public event DeadEvent DeadNotify;

        private void OnEnable()
        {
            _currentHP = MaxHP;
        }
        public void SetHp(PowerUpObject powerUpObject)
        {
            _maxHP = Mathf.RoundToInt(powerUpObject.GetPower("CoreHp", _maxHP));
            OnEnable();
        }
        public void SetTurret()
        {
            Instantiate(_turret, new Vector3(transform.position.x, transform.position.y + 1), Quaternion.identity, transform);
        }
        public void TakeDamage(int damage)
        {
            if (Dead)
                return;
            if (_currentHP <= damage)
            {
                _currentHP = 0;
                HPChanged?.Invoke(this);
                DeadNotify?.Invoke();
                return;
            }
            _currentHP -= damage;
            HPChanged?.Invoke(this);
        }
    }
}