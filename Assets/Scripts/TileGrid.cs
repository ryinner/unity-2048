using System.Linq;
using UnityEngine;

namespace Game.Tiles
{
    public class TileGrid : MonoBehaviour
    {
        public TileRow[] Rows { get; private set; }

        public TileCell[] Cells { get; private set; }

        public int Size => Cells.Length;

        public int Height => Rows.Length;

        public int Width => Rows.First().Cells.Length;

        private void Awake()
        {
            Rows = GetComponentsInChildren<TileRow>();
            Cells = GetComponentsInChildren<TileCell>();
        }

        private void Start()
        {
            for (int y = 0; y < Rows.Length; y++)
            {
                for (int x = 0; x < Rows[y].Cells.Length; x++)
                {
                    Rows[y].Cells[x].Coordinates = new Vector2Int(x, y);
                }
            }
        }
    }
}