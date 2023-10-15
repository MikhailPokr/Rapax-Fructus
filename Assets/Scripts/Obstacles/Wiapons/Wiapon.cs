using UnityEngine;

namespace RapaxFructus
{
    internal abstract class Wiapon : MonoBehaviour
    {
        /// <summary>
        /// Статы оружия.
        /// </summary>
        [SerializeField] protected WiaponStats _stats;

        /// <summary>
        /// Для доступа к улучшениям.
        /// </summary>
        [SerializeField] private PowerUpObject _powerUpObject;

        /// <summary>
        /// Скорость.
        /// </summary>
        protected float _speed;
        /// <summary>
        /// Урон.
        /// </summary>
        protected int _damage;
        /// <summary>
        /// Радиус.
        /// </summary>
        protected float _range;
        /// <summary>
        /// Откат.
        /// </summary>
        protected float _cooldown;
        /// <summary>
        /// Радиус.
        /// </summary>
        public float Range => _range;

        /// <summary>
        /// Таймер для отката.
        /// </summary>
        protected float _timer;

        /// <summary>
        /// Текущая цель.
        /// </summary>
        protected GameObject _target;
        /// <summary>
        /// Потребление энергии.
        /// </summary>
        private float _energyConsumption;
        public float EnergyConsumption => _energyConsumption;

        protected virtual void Update()
        {
            _timer += Time.deltaTime;
        }

        protected virtual void OnEnable()
        {
            _speed = Random.Range(_stats.MinSpeed, _stats.MaxSpeed);
            _damage = Random.Range(_stats.MinDamage, _stats.MaxDamage);
            _range = Random.Range(_stats.MinRange, _stats.MaxRange);
            _cooldown = Random.Range(_stats.MinCooldown, _stats.MaxCooldown);
            _energyConsumption = _stats.EnergyConsumption;

            //Система прокачки.
            if (_powerUpObject.Unlock)
            {
                _speed = _powerUpObject.GetPower("Speed", _speed);
                _damage = Mathf.RoundToInt(_powerUpObject.GetPower("Damage", _damage));
                _range = _powerUpObject.GetPower("Range", _range);
                _cooldown = _powerUpObject.GetPower("Cooldown", _cooldown);
                _energyConsumption = _powerUpObject.GetPower("Energy", _energyConsumption);
            }
        }

        /// <summary>
        /// Установливает цель.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public abstract bool SetTarget(GameObject target);
    }

    //Опять же, особых причин в отдельном скрипте под статы нет. 
}