using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RapaxFructus
{
    internal class BalanceText : MonoBehaviour
    {
        [SerializeField] private Text Text;
        private void OnEnable()
        {
            DataManager.PointsCountChange += OnPointsCountChange;   
        }

        public void OnPointsCountChange()
        {
            Text.text = "Balance: " + DataManager.EnergyPoints;
        }
        private void OnDisable()
        {
            OnDestroy();
        }
        private void OnDestroy()
        {
            DataManager.PointsCountChange -= OnPointsCountChange;
        }
    }
}