using System.Collections;
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

        [field: SerializeField, Range(0.1f, 0.5f)]
        private float AnimationDuration { get; } = 0.1f;

        private bool Waiting { get; set; } = false;

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

        private void OnUp(InputValue inputValue)
        {
            if (!Waiting)
            {
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            }
        }

        private void OnDown(InputValue inputValue)
        {
            if (!Waiting)
            {
                MoveTiles(Vector2Int.down, 0, 1, _grid.Height - 2, -1);
            }
        }

        private void OnLeft(InputValue inputValue)
        {
            if (!Waiting)
            {
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
        }

        private void OnRight(InputValue inputValue)
        {
            if (!Waiting)
            {
                MoveTiles(Vector2Int.right, _grid.Width - 2, -1, 0, 1);
            }
        }

        private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
        {
            bool isChanged = false;
            for (int x = startX; x >= 0 && x < _grid.Width; x += incrementX)
            {
                for (int y = startY; y >= 0 && y < _grid.Height; y += incrementY)
                {
                    TileCell cell = _grid.GetCell(x, y);

                    if (cell.IsOccupied)
                    {
                        isChanged |= MoveTile(cell.Tile, direction);
                    }
                }
            }

            if (isChanged)
            {
                StartCoroutine(WaitForChanges());
            }
        }

        private bool MoveTile(Tile tile, Vector2Int direction)
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
                tile.MoveTo(newCell, AnimationDuration);
                return true;
            }

            return false;
        }

        private IEnumerator WaitForChanges()
        {
            Waiting = true;

            yield return new WaitForSeconds(AnimationDuration);

            Waiting = false;

            // TODO: create new tile
            // TODO: check is game over
        }
    }
} 