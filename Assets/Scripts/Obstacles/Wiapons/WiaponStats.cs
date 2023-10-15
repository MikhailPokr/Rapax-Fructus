using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    [CreateAssetMenu(menuName = "Obstacles/Wiapon")]
    public class WiaponStats : ScriptableObject
    {
        public float EnergyConsumption;
        [Space]
        public float MinSpeed;
        public float MaxSpeed;
        [Space]
        public float MinRange;
        public float MaxRange;
        [Space]
        public int MinDamage;
        public int MaxDamage;
        [Space]
        [Header("Может не использоваться на некотором вооружении, если это так, оставить -1")]
        public float MinCooldown = -1;
        public float MaxCooldown = -1;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (MinSpeed > MaxSpeed)
                MaxSpeed = MinSpeed;
            if (MinRange > MaxRange)
                MaxRange = MinRange;
            if (MinDamage > MaxDamage)
                MaxDamage = MinDamage;
            if (MinCooldown > MaxCooldown)
                MaxCooldown = MinCooldown;
            if (MinCooldown <= 0)
            {
                MinCooldown = -1;
                MaxCooldown = -1;
            }
        }
#endif
    }
}