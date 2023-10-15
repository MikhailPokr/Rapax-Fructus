using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    public class PowerUpMenu : MonoBehaviour
    {
        private MarketElement[] _marketElements;
        private PowerUpDecor[] _powerUpDecors;

        private void Start()
        {
            foreach (var marketElement in _marketElements)
            {
                marketElement.Click += UpdateGrid;
            }
        }
        private void OnEnable()
        {
            _marketElements = GetComponentsInChildren<MarketElement>(true);
            _powerUpDecors = GetComponentsInChildren<PowerUpDecor>(true);

            UpdateGrid();
        }
        private void UpdateGrid()
        {
            foreach (MarketElement element in _marketElements)
            {
                element.gameObject.SetActive(element.Check());
                element.UpdateData();
            }
            foreach (PowerUpDecor decor in _powerUpDecors)
            {
                decor.UpdateVisibility();
            }
        }
    }
}