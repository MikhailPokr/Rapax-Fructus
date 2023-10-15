using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace RapaxFructus
{
    public class EnengyDisplay : MonoBehaviour
    {
        [SerializeField] private RectTransform _energy;
        [SerializeField] private GameObject _mark;

        public void Activate()
        {
            ManagerDirectory.Instance.Player.EnergyChanged += OnEnergyChanged;
            OnEnergyChanged();
            _mark.transform.localPosition = new Vector3(0, ManagerDirectory.Instance.Player.MinNecessaryEnergy / ManagerDirectory.Instance.Player.MaxEnergy * _energy.rect.height - _energy.rect.height / 2, 0);
        }
        private void OnEnergyChanged()
        {
            float energy = ManagerDirectory.Instance.Player.CurrentEnergy;
            float maxEnergy = ManagerDirectory.Instance.Player.MaxEnergy;

            float height = _energy.rect.height / 2;

            _energy.localScale = new Vector3(1, energy / maxEnergy, 1);
            _energy.localPosition = new Vector3(0, -height + energy / maxEnergy * height, 0);
        }
    }
}