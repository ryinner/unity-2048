using System;
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

        private TileGrid _grid;

        private List<Tile> _tiles;

        [SerializeField]
        private GameManager _gameManager;

        [field: SerializeField, Space(20)]
        public TileState[] TileStates { get; set; }

        [field: SerializeField, Range(0.1f, 0.5f)]
        private float AnimationDuration { get; } = 0.1f;

        private bool Waiting { get; set; } = false;

        public void CreateTile()
        {
            Tile tile = Instantiate(_tilePrefab, _grid.transform);
            tile.SetState(TileStates.First(), 2);
            TileCell randomEmptyCell = _grid.GetRandomEmptyCell();
            if (randomEmptyCell)
            {
                tile.Spawn(_grid.GetRandomEmptyCell());
            }

            _tiles.Add(tile);
        }

        public void Clear()
        {
            foreach (var cell in _grid.Cells)
            {
                cell.Tile = null;
            }

            foreach (var tile in _tiles)
            {
                Destroy(tile.gameObject);
            }

            _tiles.Clear();
        }

        private void Awake()
        {
            _grid = GetComponentInChildren<TileGrid>();
            _tiles = new List<Tile>();
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

            bool isChanged = false;

            int loops = 0;
            while (adjacent != null)
            {
                if (loops++ > 1000)
                {
                    break;
                }
                if (adjacent.IsOccupied)
                {
                    if (IsCanMerge(tile, adjacent.Tile))
                    {
                        isChanged = true;
                        Merge(tile, adjacent.Tile);
                    }
                    break;
                }

                newCell = adjacent;
                adjacent = _grid.GetAdjacentCell(adjacent, direction);
            }

            if (newCell != null)
            {
                tile.MoveTo(newCell, AnimationDuration);
                isChanged = true;
            }

            return isChanged;
        }

        private void Merge(Tile a, Tile b)
        {
            _tiles.Remove(a);
            a.Merge(b.Cell, AnimationDuration);

            int index = Mathf.Clamp(IndexOf(b.State) + 1, 0, TileStates.Length - 1);
            int number = b.Number * 2;

            b.SetState(TileStates[index], number);
        }

        private int IndexOf(TileState state)
        {
            for (int i = 0; i < TileStates.Length; i++)
            {
                if (state == TileStates[i])
                {
                    return i;
                }
            }

            return -1;
        }

        private bool IsCanMerge(Tile a, Tile b)
        {
            return a.Number == b.Number && !b.Locked;
        }

        private IEnumerator WaitForChanges()
        {
            Waiting = true;

            yield return new WaitForSeconds(AnimationDuration);

            Waiting = false;

            foreach (var tile in _tiles)
            {
                tile.Locked = false;
            }

            if (_tiles.Count < _grid.Size)
            {
                CreateTile();
            }

            if (CheckForGameOver())
            {
                _gameManager.GameOver();
            }
        }

        private bool CheckForGameOver()
        {
            if (_tiles.Count < _grid.Size)
            {
                return false;
            }


            foreach (var tile in _tiles)
            {
                TileCell up = _grid.GetAdjacentCell(tile.Cell, Vector2Int.up);
                if (up != null && IsCanMerge(tile, up.Tile))
                {
                    return false;
                }
                TileCell down = _grid.GetAdjacentCell(tile.Cell, Vector2Int.down);
                if (down != null && IsCanMerge(tile, down.Tile))
                {
                    return false;
                }
                TileCell left = _grid.GetAdjacentCell(tile.Cell, Vector2Int.left);
                if (left != null && IsCanMerge(tile, left.Tile))
                {
                    return false;
                }
                TileCell right = _grid.GetAdjacentCell(tile.Cell, Vector2Int.right);
                if (right != null && IsCanMerge(tile, right.Tile))
                {
                    return false;
                }
            }

            return true;
        }
    }
} 