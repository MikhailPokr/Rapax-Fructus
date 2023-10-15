using System;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// ��� ��������� �� ������� ������ ���������� ���������.
    /// </summary>
    internal enum EnemyComponent
    {
        _,
        AttackingEnemy,
        GroundEnemy,
        FlyingEnemy, //�������
        CeilingEnemy, //�������
    }
    /// <summary>
    /// ����� �� ������� ����������� � ������������� �� �����������.
    /// </summary>
    internal static class EnemyLogicPriority
    {
        //�������� � ������� ������, ����� ������������ ��� ������
        private static Dictionary<EnemyComponent, int> _dictionary = new Dictionary<EnemyComponent, int>
        {
            {EnemyComponent.CeilingEnemy, 1 },
            {EnemyComponent.FlyingEnemy, 2 },
            {EnemyComponent.GroundEnemy, 3 },
            {EnemyComponent.AttackingEnemy, 4 }
        };

        /// <summary>
        /// �������� ���������.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public static int Get(EnemyLogicComponent component)
        {
            //��������� �������� ������ ����� ����� ����������� � ������������. ���������� ��� ������� ������ � ��������.
            return _dictionary[Enum.Parse<EnemyComponent>(component.GetType().ToString().Split('.')[^1])];
        }
    }

    /// <summary>
    /// ����������� ����� ��� �����, ��� �������� ����������� �����.
    /// </summary>
    internal abstract class EnemyLogicComponent : MonoBehaviour
    {
        protected Enemy _mainScript;
        protected int _priority;
        public int Priority => _priority;

        /// <summary>
        /// �������� ��� ����������� �������� � ������������� ��.
        /// </summary>
        /// <param name="stats"></param>
        public virtual void SetStartValues(Enemy mainScript, EnemyStats stats)
        {
            _mainScript = mainScript;
            _priority = EnemyLogicPriority.Get(this);
        }
        /// <summary>
        /// ��������� ��� ���������� ��������� �����.
        /// </summary>
        public abstract void Step();
        /// <summary>
        /// ��������� ���������.
        /// </summary>
        protected virtual void Disable()
        {
            //�� ��� ��� ���. 
            _mainScript.DisableComponent(GetType());
        }
    }
    //�� ������������� �������� ����������� �� ����� � ��������� ������� ����� �������� ������ ���������� �����.
    //��� ���� ������� ������� ����� �������� ����� ����� ������, �� � �������� ����� ��� �� �����.
}