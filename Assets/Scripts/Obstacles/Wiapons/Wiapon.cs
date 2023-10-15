using UnityEngine;

namespace RapaxFructus
{
    internal abstract class Wiapon : MonoBehaviour
    {
        /// <summary>
        /// ����� ������.
        /// </summary>
        [SerializeField] protected WiaponStats _stats;

        /// <summary>
        /// ��� ������� � ����������.
        /// </summary>
        [SerializeField] private PowerUpObject _powerUpObject;

        /// <summary>
        /// ��������.
        /// </summary>
        protected float _speed;
        /// <summary>
        /// ����.
        /// </summary>
        protected int _damage;
        /// <summary>
        /// ������.
        /// </summary>
        protected float _range;
        /// <summary>
        /// �����.
        /// </summary>
        protected float _cooldown;
        /// <summary>
        /// ������.
        /// </summary>
        public float Range => _range;

        /// <summary>
        /// ������ ��� ������.
        /// </summary>
        protected float _timer;

        /// <summary>
        /// ������� ����.
        /// </summary>
        protected GameObject _target;
        /// <summary>
        /// ����������� �������.
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

            //������� ��������.
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
        /// ������������� ����.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public abstract bool SetTarget(GameObject target);
    }

    //����� ��, ������ ������ � ��������� ������� ��� ����� ���. 
}