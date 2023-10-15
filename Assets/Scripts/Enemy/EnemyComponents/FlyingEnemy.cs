using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// ����� � ���� ����������� ����� ������ ���� �����.
    /// </summary>
    internal class FlyingEnemy : EnemyLogicComponent
    {
        /// <summary>
        /// �������� ������.
        /// </summary>
        private float _flyingSpeed;
        public override void SetStartValues(Enemy mainScript, EnemyStats stats)
        {
            base.SetStartValues(mainScript, stats);
            _flyingSpeed = Random.Range(stats.FlyingSpeedMin, stats.FlyingSpeedMax);
        }

        public override void Step()
        {
            print($"������ {this.GetType().ToString()} ��������� ���.");
        }
        /// <summary>
        /// ��������� ��� ������� �� ���.
        /// </summary>
        /// <param name="WithPriority"></param>
        public void ChangeMoveSystem(int WithPriority)
        {
            enabled = WithPriority == Priority;
        }
    }
    //����� �����, ��� � � ����������� �������, �� �����.
    //�� � ������ ���� ����� ��������, ���� � ����� ����.
    //����� ������ ���������� FlyingPoint � ��������� �� ��� �������������, ��� �������� ��� � ����� ���������� �������.
    //�� ����� ������� ����� ��������� � ������ ������ ����� � ������� ������������, ��� ������ ������� ��� ������.
}