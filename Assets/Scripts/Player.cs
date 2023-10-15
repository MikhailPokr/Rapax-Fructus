using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Класс для хранения информации о игроке, а также реализующий методы его взаимодействия с игрой.
    /// </summary>
    internal class Player : MonoBehaviour
    {
        private List<Core> _cores = new List<Core>();
        public List<Core> Cores => _cores;

        private float _maxEnergy;
        public float MaxEnergy => _maxEnergy;
        private float _currentEnergy;
        public float CurrentEnergy => _currentEnergy;
        public delegate void EnergyChangedHandler();
        public event EnergyChangedHandler EnergyChanged;

        private float _minNecessaryEnergy;
        public float MinNecessaryEnergy => _minNecessaryEnergy;
        public delegate void MinEnergyMarkCrossedHandler();
        public event MinEnergyMarkCrossedHandler MinEnergyMarkCrossed;

        public delegate void GameOverEvent();
        public event GameOverEvent GameOver;

        private Dictionary<string, int> _powerUpList;
        public Dictionary<string, int> PowerUpList => _powerUpList;

        private int _specialResourseCount;
        public int SpecialResourseCount => _specialResourseCount;
        public delegate void ResourseCountChangedHandler();
        public event ResourseCountChangedHandler ResourseCountChanged;

        public void Activate(List<Core> cores)
        {
            _maxEnergy = ManagerDirectory.Instance.LevelGenerator.GetMaxEnergy();
            _minNecessaryEnergy = ManagerDirectory.Instance.LevelGenerator.GetNecessaryEnergy() + 30;
            _currentEnergy = 20;

            _cores = cores;
            foreach (Core core in _cores)
                core.DeadNotify += OnCoreDead;
        }
        public void SetPowerUpList(Dictionary<string, int> list)
        {
            _powerUpList = list;
        }

        private void OnCoreDead()
        {
            foreach (Core core in _cores)
            {
                if (!core.Dead)
                    return;
                GameOver?.Invoke();
            }
        }

        public bool GiveEnergy(float energy)
        {
            if (_currentEnergy + energy > _maxEnergy)
            {
                _currentEnergy = _maxEnergy;
                return false;
            }
            else
                _currentEnergy += energy;
            if (_currentEnergy >= _minNecessaryEnergy && _currentEnergy - energy < _minNecessaryEnergy)
                MinEnergyMarkCrossed?.Invoke();
            EnergyChanged?.Invoke();
            return true;
        }

        public bool ChangeResourseCount(int delta)
        {
            if (!DataManager.Save.CurrentGameData.PowerUps.TryGetValue("Resourses", out int level))
                level = 0;
            level += 3;
            if (_specialResourseCount + delta < 0 || _specialResourseCount > level)
                return false;
            _specialResourseCount += delta;
            ResourseCountChanged?.Invoke();
            return true;
        }

        public bool UseEnergy(float energy)
        {
            if (_currentEnergy - energy < 0)
                return false;
            _currentEnergy -= energy;
            if (_currentEnergy <= _minNecessaryEnergy && _currentEnergy + energy > _minNecessaryEnergy)
                MinEnergyMarkCrossed?.Invoke();
            EnergyChanged?.Invoke();
            return true;
        }

    }
}