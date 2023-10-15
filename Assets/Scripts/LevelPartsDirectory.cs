using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Все доступные типы уровней.
    /// </summary>
    public enum PartType
    {
        Tiny,
        Little,
        Adapter
    }
    /// <summary>
    /// Хранит в себе части уровня и декор.
    /// </summary>
    [CreateAssetMenu(menuName = "Level/LevelPartsDirectory")]
    public class LevelPartsDirectory : ScriptableObject
    {
        [Header("General Parts")]
        public GameObject Leaves;
        public GameObject NonFruit;
        [Header("Tiny Level")]
        [SerializeField] private GameObject _tinyTinyLevel;
        [SerializeField] private GameObject _tinyLevelTopDecor;
        [SerializeField] private GameObject _tinyLevelBottomDecor;
        [Header("Little Level")]
        [SerializeField] private GameObject _littleTinyLevel;
        [SerializeField] private GameObject _littleLevelTopDecor;
        [SerializeField] private GameObject _littleLevelBottomDecor;
        [Header("Adapters")]
        [SerializeField] private GameObject _tinyToTiny;
        [SerializeField] private GameObject _littleToTiny;
        [SerializeField] private GameObject _littleToLittle;

        private Dictionary<PartType, Dictionary<string, GameObject>> _parts;
        public Dictionary<PartType, Dictionary<string, GameObject>> Parts
        {
            get
            {
                _parts ??= new Dictionary<PartType, Dictionary<string, GameObject>>()
                    {
                        {PartType.Tiny, new Dictionary<string, GameObject>()
                            {
                                {"LevelTiny", _tinyTinyLevel},
                                {"Top", _tinyLevelTopDecor },
                                {"Bottom", _tinyLevelBottomDecor }
                            }
                        },
                        {PartType.Little, new Dictionary<string, GameObject>()
                            {
                                {"LevelTiny", _littleTinyLevel },
                                /*{"LevelLittle", _littleTinyLevel },*/
                                {"Top", _littleLevelTopDecor },
                                {"Bottom", _littleLevelBottomDecor }
                            }
                        },
                        {PartType.Adapter, new Dictionary<string, GameObject>()
                            {
                                {"TinyToTiny", _tinyToTiny },
                                {"LittleToTiny", _littleToTiny },
                                {"LittleToLittle", _littleToLittle },
                            }
                        }
                    };
                return _parts;
            }
        }
    }
}
