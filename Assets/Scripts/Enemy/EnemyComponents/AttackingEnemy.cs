using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Противники с этим компонентом будут атаковать то, что им указать как цель.
    /// </summary>
    internal class AttackingEnemy : EnemyLogicComponent
    {
        /// <summary>
        /// Список атак
        /// </summary>
        private List<Attack> _attacks;
        /// <summary>
        /// коэфицент, на который увеличивается скорость всех атак
        /// </summary>
        private float[] _attackSpeedCoefficient;
        /// <summary>
        /// Минимальный коэфицент урона атаки
        /// </summary>
        private float[] _damageCoefficientMin;
        /// <summary>
        /// Максимальный коэфицент урона атаки
        /// </summary>
        private float[] _damageCoefficientMax;

        /// <summary>
        /// Выбранная атака, ближняя или дальняя
        /// </summary>
        private Attack.Type _selectedDistance;
        /// <summary>
        /// Минимальный ридиус среди всех атак
        /// </summary>
        private float _minRadius;

        /// <summary>
        /// текущая выбранная атака
        /// </summary>
        private Attack _currentAttack;
        /// <summary>
        /// индекс выбранной атаки
        /// </summary>
        private int _currentAttackIndex;

        /// <summary>
        /// таймер для Step
        /// </summary>
        private float _timer;

        public override void SetStartValues(Enemy mainScript, EnemyStats stats)
        {
            base.SetStartValues(mainScript, stats);
            _attacks = stats.Attacks.ToList();
            _attackSpeedCoefficient = new float[_attacks.Count];
            _damageCoefficientMin = new float[_attacks.Count];
            _damageCoefficientMax = new float[_attacks.Count];
            for (int i = 0; i < _attacks.Count; i++)
            {
                _attackSpeedCoefficient[i] = Random.Range(stats.StrengthMin, stats.StrengthMax);
                _damageCoefficientMin[i] = stats.StrengthMin;
                _damageCoefficientMax[i] = stats.StrengthMax;

                //улучшения
                if (_mainScript.PowerUpObject.Unlock)
                {
                    _attackSpeedCoefficient[i] = _mainScript.PowerUpObject.GetPower("AttackSpeed", _attackSpeedCoefficient[i]);
                    _damageCoefficientMin[i] = _mainScript.PowerUpObject.GetPower("Damage", _damageCoefficientMin[i]);
                    _damageCoefficientMax[i] = _mainScript.PowerUpObject.GetPower("Damage", _damageCoefficientMax[i]);
                }
            }
            _mainScript.NearCoreEvent += OnNearCore;
        }

        /// <summary>
        /// Обработчик события приближения к ядру.
        /// </summary>
        private void OnNearCore()
        {
            if (_attacks.Count == 0)
                return;
            Vector3 position;
            _selectedDistance = _attacks[Random.Range(0, _attacks.Count)].AttackType;
            _minRadius = _attacks.FindAll(x => x.AttackType == _selectedDistance).Min(x => x.AttackRange);
            position = transform.position - _mainScript.DirectoryOfPoints.CorePoint.transform.position;
            position = _mainScript.DirectoryOfPoints.CorePoint.transform.position + position.normalized * _minRadius * 0.9f;
            _mainScript.AllComponentsSendMessage("SetTargetPosition", position);
            enabled = true;
        }
        public override void Step()
        {
            if (_mainScript.Animator.GetInteger("Attack") != 0)
                _mainScript.Animator.SetInteger("Attack", 0); 

            _timer += Time.deltaTime;

            if (Vector3.Distance(transform.position, _mainScript.DirectoryOfPoints.CorePoint.transform.position) <= _minRadius)
                _mainScript.Animator.SetTrigger("Stop Walking");
            else
                return;

            if (_currentAttack == null)
            {
                _timer = 0;
                _currentAttackIndex = Random.Range(0, _attacks.Count);
                _currentAttack = _attacks[_currentAttackIndex];
                _mainScript.Animator.SetInteger("Attack", _currentAttack.AttackIndex);
            }
            if (_mainScript.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == _mainScript.Stats.Name + " Idle" && _timer >= _currentAttack.AttackSpeed * _attackSpeedCoefficient[_currentAttackIndex])
            {
                _mainScript.DirectoryOfPoints.CorePoint.Core.TakeDamage(Mathf.RoundToInt(Random.Range(_currentAttack.AttackDamage * _damageCoefficientMin[_currentAttackIndex], _currentAttack.AttackDamage * _damageCoefficientMax[_currentAttackIndex])));
                _timer = 0;
                _currentAttack = _attacks[Random.Range(0, _attacks.Count)];
                _mainScript.Animator.SetInteger("Attack", _currentAttack.AttackIndex);
                
            }
        }
    }

    //Скрипт из архитектуры врагов.
    //Как и все остальные нуждается в переработке.
    //По сути не нужен, его функционал стоит переписать в основной, поскольку большая часть кода - это просто костыли для связей.
}