using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    [Serializable]
    public class PowerUpDecor : MonoBehaviour
    {
        [SerializeField] private MarketElement.MarketCondition[] _marketConditions;

        public void UpdateVisibility()
        {
            foreach (MarketElement.MarketCondition marketCondition in _marketConditions)
            {
                if (!marketCondition.Check())
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
            gameObject.SetActive(true);
        }
    }
}