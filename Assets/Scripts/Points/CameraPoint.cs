using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Точка в которую перемещается камера. 
    /// </summary>
    public class CameraPoint : MonoBehaviour
    {
        public int Line;
        public CameraMovement.CameraPosition CameraPositionType;
        public float Transparent;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 0, 1, 0.2f);
            Gizmos.DrawSphere(transform.position, 0.1f);
        }

        [ContextMenu("Поставить камеру")]
        public void MoveCamera()
        {
            Camera.main.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }
#endif
    }
}