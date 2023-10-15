
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Враги с этим компонентом могут стать особыми и давать специальный ресурс.
    /// </summary>
    // Даже не связан с системой компонентов врага, потому что такая связь создаст новые проблемы.
    internal class SpecialResourceEnemy : MonoBehaviour
    {
        /// <summary>
        /// Шанс на то, что новый враг будет иметь специальный ресурс.
        /// </summary>
        [SerializeField, Range(0,1)] private float _chance;
        /// <summary>
        /// Смена на этот материал при наличии ресурса.
        /// </summary>
        [SerializeField] private Material _material;
        /// <summary>
        /// Имеет ли ресурс.
        /// </summary>
        private bool _hasResource = false;
        /// <summary>
        /// Имеет ли ресурс.
        /// </summary>
        public bool HasResourse => _hasResource;
        /// <summary>
        /// Костыль, чтобы не срабатывал еще раз при паузе.
        /// </summary>
        private bool _alreadyEnabled = false;

        private void OnEnable()
        {
            if (_alreadyEnabled)
                return;
            _alreadyEnabled = true;
            _hasResource = Random.value < _chance;
            if (_hasResource )
            {
                GetComponent<Enemy>().ChangeMaterial( _material );
            }
        }
    }

    //Этот скрипт или переписать как часть общего или вовсе так и оставить.
}
