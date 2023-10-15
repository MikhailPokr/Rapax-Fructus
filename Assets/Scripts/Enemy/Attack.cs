using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// ����� ����������.
    /// </summary>
    [CreateAssetMenu(menuName = "Enemy/Attack")]
    internal class Attack : ScriptableObject
    {
        public enum Type
        {
            Melee,
            Range
        }
        public enum Target
        {
            Core,
            Obstacle,
            Enemy,
        }
        /// <summary>
        /// ��� ����� - �������/�������
        /// </summary>
        //����� ������� �� ������������.
        public Type AttackType;
        /// <summary>
        /// ���� ���������.
        /// </summary>
        // ������ ������ ����, �� � ����� ����� �������� ����� 3 ��������.
        public Target[] AttackTargets;
        /// <summary>
        /// ������, �� ������� ����� ����� ����� �����
        /// </summary>
        // �������� ����, �� ����� ����.
        [Space]
        public float ViewRadius;
        /// <summary>
        /// �������� �����.
        /// </summary>
        [Space]
        public float AttackSpeed;
        /// <summary>
        /// ����.
        /// </summary>
        [Space]
        public float AttackDamage;
        [Space]
        ///������ �����.
        public float AttackRange;
        /// <summary>
        /// ������ ����� ��� ���������.
        /// </summary>
        [Space]
        [Header("������ ��������������� � �������� � ���������.")]
        public int AttackIndex;
        /// <summary>
        /// �����������, ���� �����.
        /// </summary>
        public GameObject Projectile;
    }
    //� ����� ��� ��� ����, �� ����� ���� �� ������� �� ����� �����, ������������� � ���������� ����� �� Enemy
}