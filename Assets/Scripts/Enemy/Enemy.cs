using System;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Класс, стандартный для всех врагов.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    internal class Enemy : GameEntity
    {
        /// <summary>
        /// Общие характеристики.
        /// </summary>
        [SerializeField] private EnemyStats _stats;
        /// <summary>
        /// Общие характеристики.
        /// </summary>
        // используется ровно в одном месте. Надо править.
        public EnemyStats Stats => _stats;

        /// <summary>
        /// Для доступа к улучшениям.
        /// </summary>
        public PowerUpObject PowerUpObject;
        

        /// <summary>
        /// Максимальное здоровье.
        /// </summary>
        private int _maxHP;
        /// <summary>
        /// Максимальное здоровье.
        /// </summary>
        public int MaxHP => _maxHP;
        /// <summary>
        /// Текущее здоровье.
        /// </summary>
        private int _currentHP;
        /// <summary>
        /// Умер ли враг.
        /// </summary>
        public bool Dead => _currentHP == 0;

        /// <summary>
        /// Компоненты врага.
        /// </summary>
        private List<EnemyLogicComponent> _components;

        /// <summary>
        /// Дошел ли до ядра.
        /// </summary>
        private bool _nearCore = false;
        /// <summary>
        /// Дошел ли до ядра.
        /// </summary>
        //хз зачем, если нет ссылок.
        public bool NearCore => _nearCore;

        public delegate void NearCoreHandler();
        /// <summary>
        /// Ивент при подходе к ядру.
        /// </summary>
        public event NearCoreHandler NearCoreEvent;

        public delegate void DeadHandler(Enemy enemy, bool noEnergy);
        /// <summary>
        /// Ивент при смерти.
        /// </summary>
        public event DeadHandler DeadEvent;
        
        /// <summary>
        /// Аниматор. Добавляется автоматически.
        /// </summary>
        // потом можно будет сделать чтобы вручную добавлялся.
        private Animator _animator;
        /// <summary>
        /// Аниматор.
        /// </summary>
        public Animator Animator => _animator;

        [HideInInspector] public PointsDirectory DirectoryOfPoints { get; private set; }

        private void OnEnable()
        {
            if (!_alreadyEnabled)
                _alreadyEnabled = true;
            else
                return;
            _animator = GetComponent<Animator>();
            DirectoryOfPoints = GetComponentInParent<PointsDirectory>();

            //вот это надо менять на другую систему.
            _components = new List<EnemyLogicComponent>();
            foreach (Type component in _stats.RequireComponents)
            {
                EnemyLogicComponent thisComponent = (EnemyLogicComponent)gameObject.AddComponent(component);
                thisComponent.SetStartValues(this, _stats);
                EnemyLogicComponent samePriorityComponent = _components.Find(x => x.Priority == thisComponent.Priority);
                if (samePriorityComponent != null)
                    throw new Exception($"Error. Enemy component priorities conflict. {nameof(samePriorityComponent)} and {nameof(thisComponent)} on enemy {_stats.Name}");
                _components.Add(thisComponent);
                if (!_stats.EnabledComponents.Contains(thisComponent.GetType()))
                {
                    thisComponent.enabled = false;
                }
            }
            _components.Sort((x, y) => x.Priority.CompareTo(y.Priority));
            _maxHP = UnityEngine.Random.Range(_stats.HPMin, _stats.HPMax);
            if (PowerUpObject != null)
            {
                _maxHP = Mathf.RoundToInt(PowerUpObject.GetPower("HP", _maxHP));
            }
            _currentHP = _maxHP;
        }
        private void Update()
        {
            if (Dead)
                return;
            foreach (EnemyLogicComponent component in _components)
            {
                if (!_nearCore && Vector3.Distance(DirectoryOfPoints.CorePoint.transform.position, transform.position) < DirectoryOfPoints.CorePoint.Radius)
                {
                    _nearCore = true;
                    NearCoreEvent?.Invoke();
                }
                if (component.enabled)
                    component.Step();
            }
        }
        private void OnDestroy()
        {
            DeadEvent?.Invoke(this, true);
        }
        /// <summary>
        /// Отключить компонент.
        /// </summary>
        /// <param name="type"></param>
        //Очень странный момент.
        public void DisableComponent(Type type)
        {
            _components.Find(x => x.GetType() == type).enabled = false;
            foreach (Type componentType in _stats.Transitions[type])
            {
                _components.Find(x => x.GetType() == componentType).enabled = true;
            }
        }
        /// <summary>
        /// Отправляет заданное сообщение заданному методу, даже если его нет.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="value"></param>
        // Стоит ли говорить, что это в первую очередь надо убрать?
        public void AllComponentsSendMessage(string method, object value)
        {
            foreach(EnemyLogicComponent component in _components)
            {
                component.SendMessage(method, value, SendMessageOptions.DontRequireReceiver);
            }
        }

        // надо менять на единый метод.
        /// <summary>
        /// Получить урон.
        /// </summary>
        /// <param name="damage"></param>
        public void ApplyDamage(int damage)
        {
            if (Dead || damage == 0)
                return;
            if (damage < 0)
                Heal(-damage);
            if (_currentHP - damage <= 0)
                Kill();
            else
                _currentHP -= damage;
        }
        /// <summary>
        /// Полечить.
        /// </summary>
        /// <param name="heal"></param>
        public void Heal(int heal)
        {
            if (Dead || heal == 0)
                return;
            if (heal < 0)
                ApplyDamage(-heal);
            if (_currentHP + heal >= _maxHP)
                _currentHP = _maxHP;
            else
                _currentHP += heal;
        }

        /// <summary>
        /// Убить.
        /// </summary>
        public void Kill()
        {
            _currentHP = 0;
            Animator.SetTrigger("Death");
            DeadEvent.Invoke(this, false);
        }

        /// <summary>
        /// Поменять материал.
        /// </summary>
        /// <param name="material"></param>
        // метод для специального ресурса.
        public void ChangeMaterial(Material material)
        {
            MeshRenderer[] meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                float a = meshRenderer.material.color.a;
                meshRenderer.material = material;
                meshRenderer.material.color = new Color(material.color.r, material.color.g, material.color.b, a);
            }
        }

        /*protected override void OnMouseDown()
        {
            ApplyDamage(5);
        }*/
    }
}