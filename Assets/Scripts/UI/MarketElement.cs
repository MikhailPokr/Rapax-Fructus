using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RapaxFructus
{
    internal class MarketElement : MonoBehaviour
    {
        [SerializeField] private List<MarketCondition> _conditions;
        [SerializeField] private List<PowerUpInfo> _powerUpElements;
        [Space]
        [SerializeField] private Text _costText;
        [SerializeField] private LevelViewOptions[] _levels;
        [SerializeField] private PowerUpInfo _mainPowerUp;

        public delegate void ClickHandler();
        public event ClickHandler Click;

        private int _cost;

        public bool Check()
        {
            foreach (MarketCondition condition in _conditions)
            {
                if (!condition.Check())
                    return false;
            }
            return true;
        }

        public void UpdateData()
        {
            if (string.IsNullOrEmpty(_mainPowerUp.Name))
            {
                _mainPowerUp.Name = _mainPowerUp.PowerUpObject.PowerUps[0].Name;
            }
            if (_mainPowerUp.PowerUpObject.Unlock)
            {
                int level = _mainPowerUp.PowerUpObject.GetLevel(_mainPowerUp.Name);
                for (int i = 0; i < _levels.Length; i++)
                {
                    if (!_levels[i].Active)
                        _levels[i].SetActive(true);
                }
                for (int i = 0; i < level; i++)
                {
                    _levels[i].SetColor();
                }
                _cost = Mathf.RoundToInt(_mainPowerUp.PowerUpObject.GetCost(_mainPowerUp.Name));
                _costText.text = "Cost: " + _cost.ToString();
            }
            else
            {
                for (int i = 0; i < _levels.Length; i++)
                {
                    _levels[i].SetActive(false);
                    _cost = Mathf.RoundToInt(_mainPowerUp.PowerUpObject.UnlockCost);
                    _costText.text = "Cost: " + _cost.ToString();
                }
            }
        }

        public void OnClick()
        {
            if (_mainPowerUp.PowerUpObject.GetMaxLevel(_mainPowerUp.Name) <= _mainPowerUp.PowerUpObject.GetLevel(_mainPowerUp.Name))
                return;
            if (!DataManager.ChangeEnergyPoints(-_cost, true))
                return;
            foreach (PowerUpInfo powerUp in _powerUpElements)
            {
                if (powerUp.PowerUpObject.Unlock)
                {
                    if (string.IsNullOrEmpty(powerUp.Name))
                    {
                        for (int i = 0; i < powerUp.PowerUpObject.PowerUps.Length; i++)
                        {
                            DataManager.Save.CurrentGameData.AddLevel(powerUp.PowerUpObject.Name + powerUp.PowerUpObject.PowerUps[i].Name);
                        }
                    }
                    else
                        DataManager.Save.CurrentGameData.AddLevel(powerUp.PowerUpObject.Name + powerUp.Name);

                }    
                else
                    DataManager.Save.CurrentGameData.UnlockOrLock(powerUp.PowerUpObject.Name, true);
            }
            Click?.Invoke();
            DataManager.SaveAll();
            UpdateData();
        }

        /// <summary>
        /// Типы сравнений для условий.
        /// </summary>
        [Serializable]
        public enum ComparisonType
        {
            Less,
            LessOrEqual,
            Equal,
            NotEqual,
            GreaterOrEqual,
            Greater
        }

        /// <summary>
        /// Условие активности улучшения. 
        /// </summary>
        [Serializable]
        public class MarketCondition
        {
            public PowerUpObject PowerUp;
            [Space]
            [Header("Пустое имя даст проверку на открытость.")]
            public string PowerUpName;
            [Space]
            [Header("Должен ли он быть открым для выполнения условия.")]
            public bool Unlock;
            [Space]
            [Header("Проверка на уровень при наличии имени.")]
            public int Level;
            public ComparisonType LevelComparison;

            public bool Check()
            {
                if (string.IsNullOrEmpty(PowerUpName))
                {
                    return Unlock == PowerUp.Unlock;
                }
                switch (LevelComparison)
                {
                    case ComparisonType.Less:
                        {
                            return PowerUp.GetLevel(PowerUpName) < Level;
                        }
                    case ComparisonType.LessOrEqual:
                        {
                            return PowerUp.GetLevel(PowerUpName) <= Level;
                        }
                    case ComparisonType.Equal:
                        {
                            return PowerUp.GetLevel(PowerUpName) == Level;
                        }
                    case ComparisonType.GreaterOrEqual:
                        {
                            return PowerUp.GetLevel(PowerUpName) >= Level;
                        }
                    case ComparisonType.Greater:
                        {
                            return PowerUp.GetLevel(PowerUpName) > Level;
                        }
                    case ComparisonType.NotEqual:
                        {
                            return PowerUp.GetLevel(PowerUpName) != Level;
                        }
                }
                return false;
            }
        }

        [Serializable]
        public class LevelViewOptions
        {
            public int Level;
            public Image LevelImage;
            public Color Color;
            public bool Active => LevelImage.gameObject.activeSelf;

            public void SetColor()
            {
                LevelImage.color = Color;
            }
            public void SetActive(bool active)
            {
                LevelImage.gameObject.SetActive(active);
            }
        }

        [Serializable]
        public class PowerUpInfo
        {
            public PowerUpObject PowerUpObject;
            [Header("Отсутствие имени улучшит все.")]
            public string Name;
        }

    }
}