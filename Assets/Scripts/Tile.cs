using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tiles
{
    public class Tile : MonoBehaviour
    {
        public TileState State
        {
            get => _state;
            private set
            {
                _state = value;
                if (_background != null)
                {
                    _background.color = _state.BacgroundColor;
                }
                if (_text != null)
                {
                    _text.color = _state.TextColor;
                }
            }
        }

        public TileCell Cell { get; private set; }

        public int Number
        {
            get => _number;
            private set
            {
                _number = value;
                if (_text != null)
                {
                    _text.text = _number.ToString();
                }
            }
        }

        private TileState _state;

        private int _number;

        private Image _background;

        private TextMeshProUGUI _text;

        private void Awake()
        {
            _background = GetComponent<Image>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetState(TileState state, int number)
        {
            State = state;
            Number = number;
        }

        public void Spawn(TileCell cell)
        {
            if (Cell != null)
            {
                Cell.Tile = null;
            }

            Cell = cell;
            Cell.Tile = this;

            transform.position = Cell.transform.position;
        }

        public void MoveTo(TileCell cell)
        {
              if (Cell != null)
            {
                Cell.Tile = null;
            }

            Cell = cell;
            Cell.Tile = this;

            transform.position = Cell.transform.position;
        }
    }
}