using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapaxFructus
{
    /// <summary>
    /// Направление для генерации тайлов.
    /// </summary>
    public enum Direction
    {
        Up = 2,
        Left = -1,
        Right = 1,
        Down = -2,
    }

    /// <summary>
    ///  Генерирует карту из плиток в зависимости от текущего уровня.
    /// </summary>
    public class TilesGenerator : MonoBehaviour
    {
        [SerializeField] private PointsDirectory _pointsDirectory;
        [SerializeField] private List<Tile> _tiles;
        [SerializeField] private Tile _lastTile;
        [Space]
        [SerializeField] private GameObject _tileParentObject;
        [Space]
        [SerializeField] private List<TilePoint> _pointsLine1;
        [SerializeField] private TilePoint _firstTilePoint1;
        [SerializeField] private TilePoint _lastTilePoint1;
        [Space]
        [SerializeField] private List<TilePoint> _pointsLine2;
        [SerializeField] private TilePoint _firstTilePoint2;
        [SerializeField] private TilePoint _lastTilePoint2;
        [Space]
        [SerializeField] private List<TilePoint> _pointsLine3;
        [SerializeField] private TilePoint _firstTilePoint3;
        [SerializeField] private TilePoint _lastTilePoint3;
        [Space]
        [SerializeField] private List<TilePoint> _pointsLine4;
        [SerializeField] private TilePoint _firstTilePoint4;
        [SerializeField] private TilePoint _lastTilePoint4;

        public void Generate()
        {
            GenerateLine(_pointsLine1, _firstTilePoint1, _lastTilePoint1);
            GenerateLine(_pointsLine2, _firstTilePoint2, _lastTilePoint2);
            GenerateLine(_pointsLine3, _firstTilePoint3, _lastTilePoint3);
            GenerateLine(_pointsLine4, _firstTilePoint4, _lastTilePoint4);
        }
        private void GenerateLine(List<TilePoint> pointsLine, TilePoint firstTilePoint, TilePoint lastTilePoint)
        {
            List<TileInfo> tilesInfoLine = new(); 
            foreach (TilePoint point in pointsLine)
                tilesInfoLine.Add(new TileInfo(point));
            TileInfo firstTile = tilesInfoLine.Find(x => x.tilePoint == firstTilePoint);
            firstTile.directions.Add(lastTilePoint.AdjacentTilesDirection.First().Value);
            TileInfo lastTile = tilesInfoLine.Find(x => x.tilePoint == lastTilePoint);
            lastTile.directions.Add((Direction)(-(int)lastTilePoint.AdjacentTilesDirection.First().Value));
            TileInfo previousTile = new(null);
            TileInfo currentTile = firstTile;
            while (currentTile.tilePoint != lastTilePoint)
            {
                List<TileInfo> possiblePoints = new();
                foreach (TilePoint point in currentTile.tilePoint.AdjacentTilesDirection.Keys)
                {
                    if (point == previousTile.tilePoint)
                        continue;
                    TileInfo tileInfo = tilesInfoLine.Find(x => x.tilePoint == point);
                    possiblePoints.Add(tileInfo);
                }
                if (possiblePoints.Count(x => x.count < 2) != 0)
                    possiblePoints.RemoveAll(x => x.count > 1);
                currentTile.count++;
                previousTile = currentTile;
                currentTile = possiblePoints[UnityEngine.Random.Range(0, possiblePoints.Count)];
                Direction direction = previousTile.tilePoint.AdjacentTilesDirection[currentTile.tilePoint];
                if (!previousTile.directions.Contains(direction))
                {
                    previousTile.directions.Add(direction);
                    previousTile.tilePointInDirection.Add(direction, currentTile.tilePoint);
                }
                if (!currentTile.directions.Contains((Direction)(-(int)direction)))
                {
                    currentTile.directions.Add((Direction)(-(int)direction));
                    currentTile.tilePointInDirection.Add((Direction)(-(int)direction), previousTile.tilePoint);
                }
            }
            foreach (TileInfo info in tilesInfoLine)
            {
                if (info.tilePoint == lastTilePoint)
                {
                    info.tile = SetTile(info, _lastTile);
                    var points = info.tile.GetComponentsInChildren<TurretPoint>();
                    if (points.Length > 0)
                        Destroy(points[0].transform.parent.gameObject);
                }
                    
                else
                    info.tile = SetTile(info);
            }
            foreach (TileInfo info in tilesInfoLine)
            {
                info.UploadInfo(tilesInfoLine);
                _pointsDirectory.AddGroundPoints(info.tile.GetPoints());
            }
            foreach (TileInfo info in tilesInfoLine)
            {
                info.tile.GenerateConnections();
            }
        }
        private class TileInfo
        {
            public TilePoint tilePoint;
            public List<Direction> directions;
            public int count;
            public Dictionary<Direction, TilePoint> tilePointInDirection;
            public Tile tile;

            public TileInfo(TilePoint point)
            {
                tilePoint = point;
                directions = new List<Direction>();
                count = 0;
                tilePointInDirection = new();
            }

            public void UploadInfo(List<TileInfo> info)
            {
                if (tile == null)
                    return;
                TilePoint tilepoint;
                Tile tileBack = tilePointInDirection.TryGetValue(Direction.Down, out tilepoint) ? info.Find(x => x.tilePoint == tilepoint).tile : null;
                Tile tileFront = tilePointInDirection.TryGetValue(Direction.Up, out tilepoint) ? info.Find(x => x.tilePoint == tilepoint).tile : null;
                Tile tileLeft = tilePointInDirection.TryGetValue(Direction.Left, out tilepoint) ? info.Find(x => x.tilePoint == tilepoint).tile : null;
                Tile tileRight = tilePointInDirection.TryGetValue(Direction.Right, out tilepoint) ? info.Find(x => x.tilePoint == tilepoint).tile : null;
                tile.SetAdjacentTiles(tileBack, tileFront, tileLeft, tileRight);
            }
        }
        private Tile SetTile(TileInfo info, Tile tile = null)
        {
            Tile result;
            if (tile == null)
            {
                int count = info.directions.Count;
                List<Tile> tiles;
                if (count != 2)
                {
                    tiles = _tiles.FindAll(x => x.Directions.Count == count);
                }
                else
                {
                    Direction direction1 = info.directions.First();
                    Direction direction2 = info.directions.First(x => x != direction1);
                    tiles = _tiles.FindAll(x => x.Directions.Count == 2);
                    bool type = (int)direction1 + (int)direction2 == 0;
                    for (int i = 0; i < tiles.Count; i++)
                    {
                        direction1 = tiles[i].Directions.First();
                        direction2 = tiles[i].Directions.First(x => x != direction1);
                        if ((int)direction1 + (int)direction2 == 0 != type)
                        {
                            tiles.Remove(tiles[i]);
                            i--;
                        }
                    }
                }
                tile = tiles[UnityEngine.Random.Range(0, tiles.Count)];
            }
            result = Instantiate(tile, _tileParentObject.transform);
            result.transform.position += - result.GetComponentInChildren<AnchorPoint>().transform.position + info.tilePoint.transform.position;
            for (int i = 0; i < 5; i++)
            {
                if (!Enumerable.SequenceEqual(info.directions.OrderBy(x => x), result.Directions.OrderBy(x => x)))
                    result.Rotate90Degrees();
                else
                    return result;
            }
            return null;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if ((_lastTilePoint1 != null && _pointsLine1 != null && _pointsLine1.Count != 0 && !_pointsLine1.Contains(_lastTilePoint1)) ||
                (_lastTilePoint2 != null && _pointsLine2 != null && _pointsLine2.Count != 0 && !_pointsLine2.Contains(_lastTilePoint2)) ||
                (_lastTilePoint3 != null && _pointsLine3 != null && _pointsLine3.Count != 0 && !_pointsLine3.Contains(_lastTilePoint3)) ||
                (_lastTilePoint4 != null && _pointsLine4 != null && _pointsLine4.Count != 0 && !_pointsLine4.Contains(_lastTilePoint4)))
                throw new Exception("Point and list mismatch.");
        }
#endif
    }
}
