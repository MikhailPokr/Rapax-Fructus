using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Враги с этим компонентом будут ползать по земле, проходя лабиринт.
    /// </summary>
    internal class GroundEnemy : EnemyLogicComponent
    {
        /// <summary>
        /// Скорость поворота.
        /// </summary>
        //лучше не менять, раньше была вообще константой, но я решил открыть.
        //а лучше вообще перенести повыше в наследовании (если повыше вообще будет).
        [SerializeField] private float _rotationSpeed = 0.1f;

        /// <summary>
        /// Скорость ползания.
        /// </summary>
        private float _groundSpeed;

        //будем считать, что точка нужна для списка пройденных, а позиция - чтобы знать куда реально идти.
        /// <summary>
        /// Точка до которой идет враг.
        /// </summary>
        private GroundPoint _nextPoint;
        /// <summary>
        /// Позиция до которой идет враг.
        /// </summary>
        private Vector3 _nextPosition;
        /// <summary>
        /// Словарь посещенных точек для системы прохождения лабиринта.
        /// </summary>
        private readonly Dictionary<GroundPoint, int> _passedPointsCounter = new Dictionary<GroundPoint, int>();

        /// <summary>
        /// Ищет ли враг куда идти.
        /// </summary>
        private bool _stopLooking = false;

        public override void SetStartValues(Enemy mainScript, EnemyStats stats)
        {
            base.SetStartValues(mainScript, stats);
            _groundSpeed = Random.Range(stats.GroundSpeedMin, stats.GroundSpeedMax);
            if (_mainScript.PowerUpObject.Unlock)
            {
                _groundSpeed = _mainScript.PowerUpObject.GetPower("Speed", _groundSpeed);
            }
            _nextPoint = _mainScript.DirectoryOfPoints.StartGroundPoints[transform.parent.GetComponent<SpawnPoint>()];
            _nextPosition = _nextPoint.GetRandomInsideZone();

        }

        public override void Step()
        {
            if (_nextPoint == null)
                return;

            if (Vector3.Magnitude(_nextPosition - transform.position) < 0.001f)
            {
                if (_stopLooking)
                    return;
                if (_passedPointsCounter.ContainsKey(_nextPoint))
                    _passedPointsCounter[_nextPoint]++;
                else
                    _passedPointsCounter.Add(_nextPoint, 0);
                List<GroundPoint> NextPointsCounter = _nextPoint.ConnectedPoints.FindAll(x => !_passedPointsCounter.ContainsKey(x));
                if (NextPointsCounter.Count != 0)
                    _nextPoint = NextPointsCounter[Random.Range(0, NextPointsCounter.Count)];
                else
                {
                    _nextPoint = _passedPointsCounter.OrderBy(x => x.Value).First(x => _nextPoint.ConnectedPoints.Contains(x.Key)).Key;
                }
                _nextPosition = _nextPoint.GetRandomInsideZone();
            }
            transform.SetPositionAndRotation(
                Vector3.MoveTowards(transform.position, _nextPosition, _groundSpeed * Time.deltaTime),
                Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_nextPosition - transform.position, Vector3.up), _rotationSpeed));
        }

        /// <summary>
        /// Скрипт для ручной установки позиции движения (?)
        /// </summary>
        /// <param name="position"></param>
        //Возможно это для смены движения на наземную. Возможно это будет работать, но я не уверен.
        public void SetTargetPosition(Vector3 position)
        {
            _stopLooking = true;
            _nextPosition = position;
        }

        //я вспомнил что это. Страшная вещь. Переписать на обработчик события или убрать вместе с системой компонентов врага. 
        public void ChangeMoveSystem(int WithPriority)
        {
            enabled = WithPriority == Priority;
        }

        //штука для пока единственного скила.
        /// <summary>
        /// Метод для временного изменения скорости врага на определенный множитель.
        /// </summary>
        /// <param name="reductionFactor">
        /// Множитель скорости.
        /// </param>
        /// <param name="duration">
        /// Продолжительность эффекта.
        /// </param>
        /// <returns></returns>
        public IEnumerator ChangeSpeed(PowerUpObject powerUpObject)
        {
            float newSpeed = powerUpObject.GetPower("Power", _groundSpeed);
            if (newSpeed == _groundSpeed)
            {
                newSpeed = _groundSpeed * 0.84f;
            }
            float duration = powerUpObject.GetPower("Duration", 2);
            float originalSpeed = _groundSpeed;
            _groundSpeed = newSpeed;
            yield return new WaitForSeconds(duration);
            _groundSpeed = originalSpeed;
        }
    }
}