using System;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Все доступные на текущий момент компоненты поведения.
    /// </summary>
    internal enum EnemyComponent
    {
        _,
        AttackingEnemy,
        GroundEnemy,
        FlyingEnemy, //удалить
        CeilingEnemy, //удалить
    }
    /// <summary>
    /// Класс со списком компонентов и соответвующих им приоритетом.
    /// </summary>
    internal static class EnemyLogicPriority
    {
        //странная и сложная логика, стоит переработать или убрать
        private static Dictionary<EnemyComponent, int> _dictionary = new Dictionary<EnemyComponent, int>
        {
            {EnemyComponent.CeilingEnemy, 1 },
            {EnemyComponent.FlyingEnemy, 2 },
            {EnemyComponent.GroundEnemy, 3 },
            {EnemyComponent.AttackingEnemy, 4 }
        };

        /// <summary>
        /// Получить приоритет.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public static int Get(EnemyLogicComponent component)
        {
            //совершено странный способ через имена компонентов и перечисления. Переписать или удалить вместе с системой.
            return _dictionary[Enum.Parse<EnemyComponent>(component.GetType().ToString().Split('.')[^1])];
        }
    }

    /// <summary>
    /// Абстрактный класс для всего, что является компонентом врага.
    /// </summary>
    internal abstract class EnemyLogicComponent : MonoBehaviour
    {
        protected Enemy _mainScript;
        protected int _priority;
        public int Priority => _priority;

        /// <summary>
        /// Получает все необходимые значения и устанавливает их.
        /// </summary>
        /// <param name="stats"></param>
        public virtual void SetStartValues(Enemy mainScript, EnemyStats stats)
        {
            _mainScript = mainScript;
            _priority = EnemyLogicPriority.Get(this);
        }
        /// <summary>
        /// Единичный шаг компонента поведения врага.
        /// </summary>
        public abstract void Step();
        /// <summary>
        /// Отключить компонент.
        /// </summary>
        protected virtual void Disable()
        {
            //ну вот что это. 
            _mainScript.DisableComponent(GetType());
        }
    }
    //На взаимодейтвии дочерних компонентов от этого и основного скрипта врага строится логика отдельного врага.
    //Это была попытка сделать проще создание новых типов врагов, но в конечном итоге это не нужно.
}