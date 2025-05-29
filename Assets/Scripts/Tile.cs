using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tiles
{
    public class Tile : MonoBehaviour
    {
        private TileState _state;

        private int _number;
        private TileCell _cell;

        private Image _background;

        private TextMeshProUGUI _text;

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

        public TileCell Cell
        {
            get => _cell;
            private set
            {
                if (_cell != null)
                {
                    _cell.Tile = null;
                }

                _cell = value;

                if (_cell != null)
                {
                    _cell.Tile = this;
                }
            }
        }

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

        public bool Locked { get; set; } = false;

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
            Cell = cell;

            transform.position = Cell.transform.position;
        }

        public void MoveTo(TileCell cell, float duration)
        {
            Cell = cell;

            StartCoroutine(Animate(Cell.transform.position, duration));
        }

        public void Merge(TileCell cell, float duration)
        {
            StartCoroutine(Animate(Cell.transform.position, duration, true));

            cell.Tile.Locked = true;

            Cell = null;
        }

        private IEnumerator Animate(Vector3 to, float duration, bool merging = false)
        {
            float elapsed = 0f;

            Vector3 from = transform.position;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(from, to, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = to;

            if (merging)
            {
                Destroy(gameObject);
            }
        }
    }
}