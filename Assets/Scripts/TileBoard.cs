using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Tiles
{
    public class TileBoard : MonoBehaviour
    {
        [SerializeField]
        private Tile _tilePrefab;

        [field: SerializeField]
        public TileState[] TileStates { get; set; }

        private TileGrid _grid;

        private List<Tile> _tiles;

        private void Awake()
        {
            _grid = GetComponentInChildren<TileGrid>();
            _tiles = new List<Tile>();
        }

        private void Start()
        {
            CreateTile();
            CreateTile();
        }

        private void CreateTile()
        {
            Tile tile = Instantiate(_tilePrefab, _grid.transform);
            tile.SetState(TileStates.First(), 2);
            tile.Spawn(_grid.GetRandomEmptyCell());

            _tiles.Add(tile);
        }

        // private void Update()
        // {
        //     Debug.Log("update");

        //     // Можно не двигать первую строку в ряду, т.к. она уже на краю
        //     if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        //     {
        //         Debug.Log("w");
        //         MoveTiles(Vector2Int.up, 0, 1, 1, 1);
        //     }
        //     else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        //     {
        //         Debug.Log("s");
        //         MoveTiles(Vector2Int.down, 0, 1, _grid.Height - 2, -1);
        //     }
        //     else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        //     {
        //         Debug.Log("a");
        //         MoveTiles(Vector2Int.left, 1, 1, 0, 1);
        //     }
        //     else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        //     {
        //         Debug.Log("d");
        //         MoveTiles(Vector2Int.right, _grid.Width - 2, -1, 0, 1);
        //     }
        // }

        private void OnUp(InputValue inputValue)
        {
           MoveTiles(Vector2Int.up, 0, 1, 1, 1);
        }

        private void OnDown(InputValue inputValue)
        {
            MoveTiles(Vector2Int.down, 0, 1, _grid.Height - 2, -1);
        }

        private void OnLeft(InputValue inputValue)
        {
             MoveTiles(Vector2Int.left, 1, 1, 0, 1);
        }

        private void OnRight(InputValue inputValue)
        {
              MoveTiles(Vector2Int.right, _grid.Width - 2, -1, 0, 1);
        }

        private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
        {
            for (int x = startX; x >= 0 && x < _grid.Width; x += incrementX)
            {
                for (int y = startY; y >= 0 && y < _grid.Height; y += incrementY)
                {
                    TileCell cell = _grid.GetCell(x, y);

                    if (cell.IsOccupied)
                    {
                        MoveTile(cell.Tile, direction);
                    }
                }
            }
        }

        private void MoveTile(Tile tile, Vector2Int direction)
        {
            TileCell newCell = null;
            TileCell adjacent = _grid.GetAdjacentCell(tile.Cell, direction);

            int loops = 0;
            while (adjacent != null)
            {
                if (loops++ > 1000)
                {
                    break;
                }
                if (adjacent.IsOccupied)
                {
                    // TODO: merging
                    break;
                }

                newCell = adjacent;
                adjacent = _grid.GetAdjacentCell(adjacent, direction);
            }

            if (newCell != null)
            {
                tile.MoveTo(newCell);
            }
        }
    }
} 