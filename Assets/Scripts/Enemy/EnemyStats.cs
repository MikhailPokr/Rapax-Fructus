using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Хранит всю информацию о враге.
    /// </summary>
    [CreateAssetMenu(menuName = "Enemy/Stats")]
    internal class EnemyStats : ScriptableObject
    {
        /// <summary>
        /// Название противника.
        /// </summary>
        // потом надо будет убрать имя тут или в GameEntity
        public string Name;
        [Header("Компоненты, которые необходимо добавить.")]
        [SerializeField] private List<EnemyComponent> _requireComponents;
        public List<Type> RequireComponents
        {
            get
            {
                List<Type> types = new List<Type>();
                foreach (EnemyComponent component in _requireComponents)
                {
                    if (component == EnemyComponent._)
                        continue;
                    types.Add(Type.GetType("RapaxFructus." + Enum.GetName(typeof(EnemyComponent), component)));
                }
                return types;
            }
        }
        [Space]
        [Header("Все компоненты, которых нет в этом списке, будут по умолчанию выключены.")]
        [SerializeField] private List<EnemyComponent> _enabledComponents;
        public List<Type> EnabledComponents
        {
            get
            {
                List<Type> types = new List<Type>();
                foreach (EnemyComponent component in _enabledComponents)
                {
                    if (component == EnemyComponent._)
                        continue;
                    types.Add(Type.GetType("RapaxFructus." + Enum.GetName(typeof(EnemyComponent), component)));
                }
                return types;
            }
        }
        [Space]
        [Header("Параметры")]
        [Header("Основные")]
        public int HPMax;
        public int HPMin;
        [Header("Для AttackingEnemy")]
        public Attack[] Attacks;
        [Range(0, 1)] public float StrengthMax;
        [Range(0, 1)] public float StrengthMin;
        [Header("Для GroundEnemy")]
        public float GroundSpeedMax;
        public float GroundSpeedMin;
        [Header("Для FlyingEnemy")]
        public float FlyingSpeedMax;
        public float FlyingSpeedMin;
        [Header("Для CeilingEnemy")]
        public float CeilingSpeedMax;
        public float CeilingSpeedMin;
        [Space, Space]
        [Header("Если вы выключится компонент из верхнего списка, будет включен соответствующий ему компонент из нижнего.")]
        [SerializeField] private List<EnemyComponent> _whenTurnedOff;
        [SerializeField] private List<EnemyComponent> _willBeTurnedOn;
        public Dictionary<Type, List<Type>> Transitions
        {
            get
            {
                List<Type> whenTurnedOff = new List<Type>();
                foreach (EnemyComponent component in _whenTurnedOff)
                {
                    if (component == EnemyComponent._)
                        continue;
                    whenTurnedOff.Add(Type.GetType("RapaxFructus." + Enum.GetName(typeof(EnemyComponent), component)));
                }

                List<Type> willBeTurnedOn = new List<Type>();
                foreach (EnemyComponent component in _willBeTurnedOn)
                {
                    if (component == EnemyComponent._)
                        continue;
                    willBeTurnedOn.Add(Type.GetType("RapaxFructus." + Enum.GetName(typeof(EnemyComponent), component)));
                }

                Dictionary<Type, List<Type>> result = new Dictionary <Type, List<Type>>(); 
                for (int i = 0; i < whenTurnedOff.Count; i++)
                {
                    if (!result.ContainsKey(whenTurnedOff[i]))
                        result.Add(whenTurnedOff[i], new List<Type>());
                    result[whenTurnedOff[i]].Add(willBeTurnedOn[i]);
                }
                return result;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < _enabledComponents.Count(); i++)
            {
                if (!_requireComponents.Contains(_enabledComponents[i]) && _enabledComponents[i] != EnemyComponent._)
                {
                    _requireComponents.Add(_enabledComponents[i]);
                }
            }

            if (_whenTurnedOff.Count > _willBeTurnedOn.Count)
            {
                _willBeTurnedOn.Add(EnemyComponent._);
            }
            else if (_whenTurnedOff.Count < _willBeTurnedOn.Count)
            {
                _willBeTurnedOn.Remove(_willBeTurnedOn.Last());
            }
        }
#endif
    }

    //В целом система очень сложная и как и в случае с компонентами нужно полностью переработать. 
}