using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Точка спавна врагов.
    /// </summary>    
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private float _radius;
        public float Radius => _radius;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (MeshForGizmos.Instance == null)
                return;
            Gizmos.color = new Color(1, 1, 1, 0.3f);
            Gizmos.DrawMesh(MeshForGizmos.Instance.Cylinder, transform.position, Quaternion.identity, new Vector3(_radius * 2, 0.01f, _radius * 2));        
        }
#endif
    }
}