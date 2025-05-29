using System.Collections;
using Game.Tiles;
using TMPro;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private TileBoard _board;

        [SerializeField]
        private CanvasGroup _gameOver;

        [SerializeField]
        private TextMeshProUGUI _scoreText;

        [SerializeField]
        private TextMeshProUGUI _bestScoreText;

        private int _score = 0;

        [field: SerializeField, Range(0.1f, 1f)]
        public float AnimationDuration { get; private set; } = 0.5f;

        private int Score
        {
            get => _score;
            set
            {
                _score = value;
                _scoreText.text = _score.ToString();
                SaveBestScore();
            }
        }

        public void NewGame()
        {
            Score = 0;
            _bestScoreText.text = LoadBestScore().ToString();

            _gameOver.alpha = 0;
            _gameOver.interactable = false;

            _board.Clear();
            _board.CreateTile();
            _board.CreateTile();
            _board.enabled = true;
        }

        public void GameOver()
        {
            _board.enabled = false;
            _gameOver.interactable = true;

            StartCoroutine(Fade(_gameOver, 1f, 1f));
        }

        public void IncreaseScore(int points)
        {
            Score += points;
        }

        private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
        {
            yield return new WaitForSeconds(delay);

            float elapsed = 0f;
            float from = canvasGroup.alpha;

            while (elapsed < AnimationDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / AnimationDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1;
        }

        private void Start()
        {
            NewGame();
        }

        private void SaveBestScore()
        {
            int bestScore = LoadBestScore();

            if (Score > bestScore)
            {
                PlayerPrefs.SetInt("bestScore", Score);
            }
        }

        private int LoadBestScore()
        {
            return PlayerPrefs.GetInt("bestScore", 0);
        }
    }
}

