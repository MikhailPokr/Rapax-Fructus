using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Точка связки элементов фрукта.
    /// </summary>
    public class AnchorPoint : MonoBehaviour
    {
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 0, 0.2f);
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
#endif
    }
}