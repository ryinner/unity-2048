using UnityEngine;

namespace Game.Tiles
{
    public class TileRow : MonoBehaviour
    {
        public TileCell[] Cells { get; private set; }

        private void Awake()
        {
            Cells = GetComponentsInChildren<TileCell>();
        }
    }
}