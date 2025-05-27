using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        }
    }
} 