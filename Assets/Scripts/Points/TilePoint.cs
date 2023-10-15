using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Точка, где будет стоять тайл.
    /// </summary>
    public class TilePoint : MonoBehaviour
    {

        [SerializeField] private TilePoint _tilePointBack;
        [Space]
        [SerializeField] private TilePoint _tilePointFront;
        [Space]
        [SerializeField] private TilePoint _tilePointLeft;
        [Space]
        [SerializeField] private TilePoint _tilePointRight;

        public List<TilePoint> TilePoints
        {
            get
            {
                List<TilePoint> result = new();
                if (_tilePointBack != null) result.Add(_tilePointBack);
                if (_tilePointFront != null) result.Add(_tilePointFront);
                if (_tilePointLeft != null) result.Add(_tilePointLeft);
                if (_tilePointRight != null) result.Add(_tilePointRight);

                return result;
            }
        }
             
        public Dictionary<TilePoint, Direction> AdjacentTilesDirection
        {
            get
            {
                Dictionary<TilePoint, Direction> result = new();
                if (_tilePointBack != null) result.Add(_tilePointBack, Direction.Down);
                if (_tilePointFront != null) result.Add(_tilePointFront, Direction.Up);
                if (_tilePointLeft != null) result.Add(_tilePointLeft, Direction.Left);
                if (_tilePointRight != null) result.Add(_tilePointRight, Direction.Right);

                return result;
            }
        }
#if UNITY_EDITOR

        [SerializeField] private float _radius;
        private void OnDrawGizmos/*Selected*/()
        {
            Gizmos.color = new Color(0, 0, 1, 0.2f);
            Gizmos.DrawCube(transform.position, new Vector3(_radius * 2, 0.01f, _radius * 2));
            if (_tilePointBack != null)
                Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y, transform.position.z - _radius / 2), new Vector3(_radius / 5, 0.01f, _radius));
            if (_tilePointFront != null)
                Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y, transform.position.z + _radius / 2), new Vector3(_radius / 5, 0.01f, _radius));
            if (_tilePointLeft != null)
                Gizmos.DrawCube(new Vector3(transform.position.x - _radius / 2, transform.position.y, transform.position.z), new Vector3(_radius, 0.01f, _radius / 5));
            if (_tilePointRight != null)
                Gizmos.DrawCube(new Vector3(transform.position.x + _radius / 2, transform.position.y, transform.position.z), new Vector3(_radius, 0.01f, _radius / 5));
        }

        [ContextMenu("Повернуть")]
        public void Rotation90()
        {
            TilePoint tileBack = _tilePointBack;
            TilePoint tileFront = _tilePointFront;
            TilePoint tileLeft = _tilePointLeft;
            TilePoint tileRight = _tilePointRight;
            _tilePointBack = null;
            _tilePointFront = null;
            _tilePointLeft = null;
            _tilePointRight = null;

            if (tileBack != null)
                _tilePointLeft = tileBack;
            if (tileFront != null)
                _tilePointRight = tileFront;
            if (tileLeft != null)
                _tilePointFront = tileLeft;
            if (tileRight != null)
                _tilePointBack = tileRight;
        }
#endif

    }
}