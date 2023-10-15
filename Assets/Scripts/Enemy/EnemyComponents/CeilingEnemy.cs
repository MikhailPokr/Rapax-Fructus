using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    ///  ����� � ���� ����������� ����� ������� �� ������� � ������, ���� �� �� ������.
    /// </summary>
    internal class CeilingEnemy : EnemyLogicComponent
    {
        /// <summary>
        /// �������� �������� �� �������.
        /// </summary>
        private float _ceilingSpeed;
        public override void SetStartValues(Enemy mainScript, EnemyStats stats)
        {
            base.SetStartValues(mainScript, stats);
            _ceilingSpeed = Random.Range(stats.CeilingSpeedMin, stats.CeilingSpeedMin);
        }

        public override void Step()
        {
            print($"������ {this.GetType().ToString()} ��������� ���.");
        }
        /// <summary>
        /// �������� ��������� ��� ���������� ����� ������� �� ���.
        /// </summary>
        /// <param name="WithPriority"></param>
        public void ChangeMoveSystem(int WithPriority)
        {
            enabled = WithPriority == Priority;
        }
    }
    //������ ��� ������ ������ � ����������, �� ��� � �� ��� �����������.
    //�������� ����� �������, ��������� ������� ������� ������. 
}