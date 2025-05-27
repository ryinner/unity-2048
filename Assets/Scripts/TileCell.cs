using UnityEngine;

namespace Game.Tiles
{
    public class TileCell : MonoBehaviour
    {
        public Vector2Int Coordinates { get; set; }

        public Tile Tile { get; set; }

        public bool IsEmpty => Tile == null;

        public bool IsOccupied => Tile != null;
    }
}