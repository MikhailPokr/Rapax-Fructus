using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// ����� � ���� ����������� ����� ������� �� �����, ������� ��������.
    /// </summary>
    internal class GroundEnemy : EnemyLogicComponent
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        //����� �� ������, ������ ���� ������ ����������, �� � ����� �������.
        //� ����� ������ ��������� ������ � ������������ (���� ������ ������ �����).
        [SerializeField] private float _rotationSpeed = 0.1f;

        /// <summary>
        /// �������� ��������.
        /// </summary>
        private float _groundSpeed;

        //����� �������, ��� ����� ����� ��� ������ ����������, � ������� - ����� ����� ���� ������� ����.
        /// <summary>
        /// ����� �� ������� ���� ����.
        /// </summary>
        private GroundPoint _nextPoint;
        /// <summary>
        /// ������� �� ������� ���� ����.
        /// </summary>
        private Vector3 _nextPosition;
        /// <summary>
        /// ������� ���������� ����� ��� ������� ����������� ���������.
        /// </summary>
        private readonly Dictionary<GroundPoint, int> _passedPointsCounter = new Dictionary<GroundPoint, int>();

        /// <summary>
        /// ���� �� ���� ���� ����.
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
        /// ������ ��� ������ ��������� ������� �������� (?)
        /// </summary>
        /// <param name="position"></param>
        //�������� ��� ��� ����� �������� �� ��������. �������� ��� ����� ��������, �� � �� ������.
        public void SetTargetPosition(Vector3 position)
        {
            _stopLooking = true;
            _nextPosition = position;
        }

        //� �������� ��� ���. �������� ����. ���������� �� ���������� ������� ��� ������ ������ � �������� ����������� �����. 
        public void ChangeMoveSystem(int WithPriority)
        {
            enabled = WithPriority == Priority;
        }

        //����� ��� ���� ������������� �����.
        /// <summary>
        /// ����� ��� ���������� ��������� �������� ����� �� ������������ ���������.
        /// </summary>
        /// <param name="reductionFactor">
        /// ��������� ��������.
        /// </param>
        /// <param name="duration">
        /// ����������������� �������.
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