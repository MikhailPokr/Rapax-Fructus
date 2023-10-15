
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// ����� � ���� ����������� ����� ����� ������� � ������ ����������� ������.
    /// </summary>
    // ���� �� ������ � �������� ����������� �����, ������ ��� ����� ����� ������� ����� ��������.
    internal class SpecialResourceEnemy : MonoBehaviour
    {
        /// <summary>
        /// ���� �� ��, ��� ����� ���� ����� ����� ����������� ������.
        /// </summary>
        [SerializeField, Range(0,1)] private float _chance;
        /// <summary>
        /// ����� �� ���� �������� ��� ������� �������.
        /// </summary>
        [SerializeField] private Material _material;
        /// <summary>
        /// ����� �� ������.
        /// </summary>
        private bool _hasResource = false;
        /// <summary>
        /// ����� �� ������.
        /// </summary>
        public bool HasResourse => _hasResource;
        /// <summary>
        /// �������, ����� �� ���������� ��� ��� ��� �����.
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

    //���� ������ ��� ���������� ��� ����� ������ ��� ����� ��� � ��������.
}
