using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// ������ ��������� ��� ��������. ������ ���� ������ ����. ��� ����� �������� � SaveFileManager.
    /// </summary>
    [CreateAssetMenu(menuName = "Other/PowerUpList")]
    internal class PowerUpList : ScriptableObject
    {
        public static PowerUpList Instance;
        public static void SetInstance(PowerUpList list)
        {
            Instance = list;
        }

        public List<PowerUpObject> PowerUpElements;
        public List<PowerUpObject> Locked => PowerUpElements.FindAll(x => !x.Unlock);
    }
}