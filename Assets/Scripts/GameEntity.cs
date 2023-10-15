using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Абстрактный класс для всех объектов, имеющее физическое представление в игре.  
    /// </summary>
    internal abstract class GameEntity : MonoBehaviour
    {
        [SerializeField] protected string _name;
        public string Name
        { 
            get
            { 
                return string.IsNullOrEmpty(_name) ? name : _name;
            }
        }
        [SerializeField] protected string _description;
        public Color UIColor;
        protected bool _alreadyEnabled = false;

        protected virtual void OnMouseDown()
        {
            
        }
        protected virtual void Start()
        {
            ManagerDirectory.Instance.TimeStop.TimeStopNotify += OnPause;
        }
        public void OnPause(bool stop)
        {
            if (this != null && gameObject != null)
            {
                var gameobjects = gameObject.GetComponentsInChildren<Behaviour>(); 
                foreach (var tranform in gameobjects)
                {
                    tranform.enabled = !stop;
                }
            }  
        }
    }
}