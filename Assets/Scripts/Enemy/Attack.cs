using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Атака противника.
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
        /// Тип атаки - ближний/дальний
        /// </summary>
        //вроде дальний не используется.
        public Type AttackType;
        /// <summary>
        /// Кого атаковать.
        /// </summary>
        // Сейчас только ядро, но в целом можно оставить такие 3 варианта.
        public Target[] AttackTargets;
        /// <summary>
        /// Радиус, на котором можно атака видит врага
        /// </summary>
        // Странная тема, на самом деле.
        [Space]
        public float ViewRadius;
        /// <summary>
        /// Скорость атаки.
        /// </summary>
        [Space]
        public float AttackSpeed;
        /// <summary>
        /// Урон.
        /// </summary>
        [Space]
        public float AttackDamage;
        [Space]
        ///Радиус атаки.
        public float AttackRange;
        /// <summary>
        /// Индекс атаки для аниматора.
        /// </summary>
        [Space]
        [Header("Должен соответствовать с индексом в Аниматоре.")]
        public int AttackIndex;
        /// <summary>
        /// Проджектайл, если нужен.
        /// </summary>
        public GameObject Projectile;
    }
    //в целом тут все норм, но можно было бы сделать из этого класс, настраиваемый в инспекторе прямо из Enemy
}