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

        public TileCell GetRandomEmptyCell()
        {
            int index = Random.Range(0, Cells.Length);
            int startingIndex = index;

            while (Cells[index].IsOccupied)
            {
                index++;

                if (index > Cells.Length)
                {
                    index = 0;
                }

                if (startingIndex == index)
                {
                    return null;   
                }
            }

            return Cells[index];
        }

        public TileCell GetCell(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return Rows[y].Cells[x];
            }
            return null;
        }

        public TileCell GetCell(Vector2Int coordinates)
        {
            return GetCell(coordinates.x, coordinates.y);
        }

        public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
        {
            Vector2Int coordinates = cell.Coordinates;
            coordinates.x += direction.x;
            coordinates.y -= direction.y;

            return GetCell(coordinates);
        }

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