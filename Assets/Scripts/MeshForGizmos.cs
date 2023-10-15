#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Скрипт для эдитора для хранения мешей для гизмос.
    /// </summary>
    public class MeshForGizmos : SingletonBase<MeshForGizmos>
    {
        public Mesh Cube;
        public Mesh Cylinder;
        public Mesh Sphere;
        protected override void Awake()
        {
            if (transform.root.gameObject != gameObject)
                Destroy(this);
            else
                base.Awake();
        }
    }
}
#endif