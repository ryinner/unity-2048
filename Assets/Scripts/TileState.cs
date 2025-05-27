using UnityEngine;

namespace Game.Tiles
{
    [CreateAssetMenu(menuName = "Tile state")]
    public class TileState : ScriptableObject
    {
        [field: SerializeField]
        public Color BacgroundColor { get; set; }

        [field: SerializeField]
        public Color TextColor { get; set; }
    }

}