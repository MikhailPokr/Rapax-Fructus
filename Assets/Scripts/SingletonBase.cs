using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Абстрактный класс для реализации патерна Singleton (Одиночка).
    /// </summary>
    /// <typeparam name="T">
    /// Класс из которого нужно сделать "одиночку"
    /// </typeparam>
    [DisallowMultipleComponent]
    public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        [Header("Singleton")]
        [SerializeField] private bool _doNotDestroyOnLoad;

        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("MonoSingleton: object of type already exists, instance will be destroyed = " + typeof(T).Name);
                Destroy(this);
                return;
            }

            Instance = this as T;

            if (_doNotDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
    }
}