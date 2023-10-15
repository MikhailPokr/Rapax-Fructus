using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// ����� �������, ������� ����� ��������.
    /// </summary>
    internal class PowerUpObject : MonoBehaviour
    {
        [SerializeField] private string _name;
        public string Name
        { 
            get
            {
                if (!string.IsNullOrEmpty(_name))
                    return _name;
                if (TryGetComponent(out GameEntity gameEntity))
                {
                    return gameEntity.Name.Replace(" ", "");
                }
                else
                    new Exception($"Where is the name of the object {name}? ");
                return "!!!";
            }
        }
        [Space]
        [SerializeField] private bool Openable;
        public int UnlockCost;
        public bool Unlock => !Openable || DataManager.Save.CurrentGameData.Unlocks.Contains(Name);
        [Space]
        [SerializeField] private int _baseCost;
        public int BaseCost => _baseCost;
        [Space]
        public PowerUpParameters[] PowerUps;
        /// <summary>
        /// �������� ������� ��������� ����� �������.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetLevel(string name)
        {
            PowerUpParameters powerUp = PowerUps.First(x => x.Name == name);
            if (powerUp != null) 
            {
                int level = DataManager.Save.CurrentGameData.PowerUps[Name + name];
                if (level < 0)
                    level = 0;
                return level;
            }
            return 0;
        }

        public float GetPower(string name, float baseDefault) => PowerUps.First(x => x.Name == name).GetPower(baseDefault, GetLevel(name));
        public float GetCost(string name) => PowerUps.First(x => x.Name == name).GetCost(GetLevel(name), _baseCost);
        public int GetMaxLevel(string name) => PowerUps.First(x => x.Name == name).MaxLevel;


        /// <summary>
        /// ����� ���������� ��������� �������.
        /// </summary>
        [Serializable]
        public class PowerUpParameters
        {
            [Header("������������ ��� ������� + ��� ���")]
            public string Name;
            [Header("��� ��������� ������� ������ ������� ������� ������ ����")]
            public List<LevelInfo> Levels;

            public int MaxLevel => int.Parse(Levels.Max(x => x.Level));

            /// <summary>
            /// �������� �������� ����.
            /// </summary>
            /// <param name="baseDefault"></param>
            /// <param name="level"></param>
            /// <returns></returns>
            public float GetPower(float baseDefault, int level)
            {
                if (level == 0)
                {
                    return baseDefault;
                }
                float result = baseDefault;
                int n = 0;
                int l = 0;
                while (true)
                {
                    if (level > MaxLevel)
                        level = MaxLevel;
                    while (Levels.Find(x => x.LevelInt == n) == null)
                        n++;
                    while (l != n + 1)
                    {
                        result = Levels.Find(x => x.LevelInt == n).GetPower(baseDefault, result);
                        l++;
                        if (level < l)
                            return result;
                    }
                    n++;
                }
            }

            /// <summary>
            /// �������� �������� ����.
            /// </summary>
            /// <param name="level"></param>
            /// <returns></returns>
            public float GetCost(int level, int cost)
            {
                float result = 0;
                int n = 0;
                int l = 0;
                while (true)
                {
                    if (level > MaxLevel)
                        level = MaxLevel;
                    while (Levels.Find(x => x.LevelInt == n) == null)
                        n++;
                    while (l != n + 1)
                    {
                        result = Levels.Find(x => x.LevelInt == n).GetCost(cost, result);
                        l++;
                        if (level < l)
                            return result;
                    }
                    n++;
                }
            }

            /// <summary>
            /// ���������� �� ������. � ���� Increase � Cost ��������� �������. 
            /// </summary>
            [Serializable]
            public class LevelInfo
            {
                public string Level;
                [Header("B - ������� ��������, C - �������, L - �������")]
                public string Increase;
                public string Cost;

                public int LevelInt
                {
                    get
                    {
                        if (!int.TryParse(Level, out int result))
                        {
                            result = -1;
                        }
                        return result;
                    }
                }

                /// <summary>
                /// �������� ���� �� ���� ���������� ������.
                /// </summary>
                /// <param name="baseDefault"></param>
                /// <param name="current"></param>
                /// <returns></returns>
                public float GetPower(float baseDefault, float current)
                {
                    if (Increase.Contains("�"))
                        print(Level);
                    string expression = Increase;
                    expression = expression.Replace("B", baseDefault.ToString(CultureInfo.InvariantCulture));
                    expression = expression.Replace("C", current.ToString(CultureInfo.InvariantCulture));
                    expression = expression.Replace("L", LevelInt.ToString());
                    return Utils.GetExpressionResult(expression);
                }

                /// <summary>
                /// �������� ���� �� ���� ���������� �������.
                /// </summary>
                /// <param name="baseDefault"></param>
                /// <param name="current"></param>
                /// <returns></returns>
                public float GetCost(float baseDefault, float current)
                {
                    if (Increase.Contains("�"))
                        print(Level);
                    string expression = Cost;
                    expression = expression.Replace("B", baseDefault.ToString(CultureInfo.InvariantCulture));
                    expression = expression.Replace("C", current.ToString(CultureInfo.InvariantCulture));
                    expression = expression.Replace("L", LevelInt.ToString());
                    return Utils.GetExpressionResult(expression);
                }

                private string _name => Level;
            }
        }
    }
}