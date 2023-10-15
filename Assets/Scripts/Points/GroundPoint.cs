using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// “очка, по которой будут ходить наземные враги.
    /// </summary>
    public class GroundPoint : MonoBehaviour
    {
        [SerializeField] private float _radius;
        [SerializeField] private List<GroundPoint> _connectedPoints;
        public List<GroundPoint> ConnectedPoints => _connectedPoints;
        public float Radius => _radius;

        public void SetNewConnection(GroundPoint point)
        {
            _connectedPoints.Add(point);
        }
        public Vector3 GetRandomInsideZone()
        {
            Vector3 vector = transform.position + Random.insideUnitSphere * _radius;
            vector.y = transform.position.y;
            return vector;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (MeshForGizmos.Instance == null)
                return;
            Gizmos.color = new Color(1, 0.6f, 0, 0.3f);
            Gizmos.DrawMesh(MeshForGizmos.Instance.Cylinder, transform.position, Quaternion.identity, new Vector3(_radius, 0.01f, _radius));
            Gizmos.DrawMesh(MeshForGizmos.Instance.Cylinder, transform.position, Quaternion.identity, new Vector3(_radius * 2, 0.01f, _radius * 2));
            if (_connectedPoints != null && _connectedPoints.Count != 0)
            {
                for (int i = 0; i < _connectedPoints.Count; i++)
                {
                    if (_connectedPoints[i] == null)
                        return;

                    Vector3 vector = _connectedPoints[i].transform.position - transform.position;

                    float ax;
                    float ay = vector.y;
                    float az = 1;
                    ax = (-vector.y * ay - vector.z * az) / vector.x;
                    if (ax == float.PositiveInfinity)
                        ax = (-vector.y * ay - vector.z * az) / 0.01f;
                    Vector3 vectorA = new(ax, ay, az);
                    Vector3 pointA1 = transform.position + vectorA.normalized * _radius/2;
                    Vector3 pointA2 = transform.position + vectorA.normalized * -_radius/2;

                    float bx;
                    float by = vector.y;
                    float bz = 1;
                    bx = (-vector.y * by - vector.z * bz) / vector.x;
                    if (bx == float.PositiveInfinity)
                        bx = (-vector.y * by - vector.z * bz) / 0.01f;
                    Vector3 vectorB = new(bx, by, bz);
                    Vector3 pointB1 = _connectedPoints[i].transform.position + vectorB.normalized * _connectedPoints[i].Radius/2;
                    Vector3 pointB2 = _connectedPoints[i].transform.position + vectorB.normalized * -_connectedPoints[i].Radius/2;

                    Gizmos.color = new Color(0.6f, 0.4f, 0, 0.4f);

                    Gizmos.DrawLine(pointA1, pointB1);
                    Gizmos.DrawLine(pointA2, pointB2);
                }
            }
                
        }
#endif
    }
}