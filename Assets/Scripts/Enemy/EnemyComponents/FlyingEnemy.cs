using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Враги с этим компонентом будут летать пока могут.
    /// </summary>
    internal class FlyingEnemy : EnemyLogicComponent
    {
        /// <summary>
        /// Скорость полета.
        /// </summary>
        private float _flyingSpeed;
        public override void SetStartValues(Enemy mainScript, EnemyStats stats)
        {
            base.SetStartValues(mainScript, stats);
            _flyingSpeed = Random.Range(stats.FlyingSpeedMin, stats.FlyingSpeedMax);
        }

        public override void Step()
        {
            print($"Скрипт {this.GetType().ToString()} выполняет шаг.");
        }
        /// <summary>
        /// Заготовка для падения на пол.
        /// </summary>
        /// <param name="WithPriority"></param>
        public void ChangeMoveSystem(int WithPriority)
        {
            enabled = WithPriority == Priority;
        }
    }
    //Точно также, как и с потолочными врагами, не вышло.
    //Но в теории этих можно доделать, идея в целом норм.
    //Нужно просто расставить FlyingPoint и заставить по ним передвигаться, как наземных или с более интересной логикой.
    //Но лучше конечно вовсе перенести в другой скрипт рядом с обычным перемещением, это уберет костыли для связей.
}