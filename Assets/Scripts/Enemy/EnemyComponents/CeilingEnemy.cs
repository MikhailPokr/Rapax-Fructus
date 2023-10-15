using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    ///  Враги с этим компонентом будут ползать по потолку к центру, пока их не собьют.
    /// </summary>
    internal class CeilingEnemy : EnemyLogicComponent
    {
        /// <summary>
        /// Скорость ползания по потолку.
        /// </summary>
        private float _ceilingSpeed;
        public override void SetStartValues(Enemy mainScript, EnemyStats stats)
        {
            base.SetStartValues(mainScript, stats);
            _ceilingSpeed = Random.Range(stats.CeilingSpeedMin, stats.CeilingSpeedMin);
        }

        public override void Step()
        {
            print($"Скрипт {this.GetType().ToString()} выполняет шаг.");
        }
        /// <summary>
        /// Странная заготовка для реализации смены потолка на пол.
        /// </summary>
        /// <param name="WithPriority"></param>
        public void ChangeMoveSystem(int WithPriority)
        {
            enabled = WithPriority == Priority;
        }
    }
    //Скрипт был создан вместе с остальными, но так и не был использован.
    //Возможно стоит удалить, поскольку потолок немного мешает. 
}