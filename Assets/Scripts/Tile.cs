using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Скрипт, описывающий вариант части лабиринта для последующей генерации из них.
    /// </summary>
    public class Tile : MonoBehaviour
    {
        [Header("Pass Back")]
        [SerializeField] private GroundPoint _pointBackPass;
        private Tile _adjacentTileBackPass;
        [Header("Pass Front")]
        [SerializeField] private GroundPoint _pointFrontPass;
        private Tile _adjacentTileFrontPass;
        [Header("Pass Left")]
        [SerializeField] private GroundPoint _pointLeftPass;
        private Tile _adjacentTileLeftPass;
        [Header("Pass Right")]
        [SerializeField] private GroundPoint _pointRightPass;
        private Tile _adjacentTileRightPass;
        readonly public Dictionary<Tile, GroundPoint> TileDirectory = new();

        public List<Direction> Directions
        {
            get
            {
                List<Direction> directions = new();
                if (_pointBackPass != null)
                    directions.Add(Direction.Down);
                if (_pointFrontPass != null)
                    directions.Add(Direction.Up);
                if (_pointLeftPass != null)
                    directions.Add(Direction.Left);
                if (_pointRightPass != null)
                    directions.Add(Direction.Right);
                return directions;
            }
        }

        public void GenerateConnections()
        {
            foreach (Tile tile in TileDirectory.Keys)
            {
                if (tile == null)
                    continue;
                tile.SetConnection(this, TileDirectory[tile]);
            }
        }
        public void SetAdjacentTiles(Tile tileBack, Tile tileFront, Tile tileLeft, Tile tileRight)
        {
            _adjacentTileBackPass = tileBack;
            _adjacentTileFrontPass = tileFront;
            _adjacentTileLeftPass = tileLeft;
            _adjacentTileRightPass = tileRight;

            if (_adjacentTileBackPass != null)
                TileDirectory.Add(_adjacentTileBackPass, _pointBackPass);
            if (_adjacentTileFrontPass != null)
                TileDirectory.Add(_adjacentTileFrontPass, _pointFrontPass);
            if (_adjacentTileLeftPass != null)
                TileDirectory.Add(_adjacentTileLeftPass, _pointLeftPass);
            if (_adjacentTileRightPass != null)
                TileDirectory.Add(_adjacentTileRightPass, _pointRightPass);
        }
        public void Rotate90Degrees()
        {
            GroundPoint pointBack = _pointBackPass;
            GroundPoint pointFront = _pointFrontPass;
            GroundPoint pointLeft = _pointLeftPass;
            GroundPoint pointRight = _pointRightPass;
            _pointBackPass = null;
            _pointFrontPass = null;
            _pointLeftPass = null;
            _pointRightPass = null;

            if (pointBack != null)
                _pointLeftPass = pointBack;
            if (pointFront != null)
                _pointRightPass = pointFront;
            if (pointLeft != null)
                _pointFrontPass = pointLeft;
            if (pointRight != null)
                _pointBackPass = pointRight;

            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y + 90, transform.localRotation.eulerAngles.z);
        }

        public List<GroundPoint> GetPoints()
        {
            return GetComponentsInChildren<GroundPoint>().ToList();
        }
        public void SetConnection(Tile tile, GroundPoint point)
        {
            TileDirectory[tile].SetNewConnection(point);
        }
    }
}