using System.Collections;
using TMPro;
using UnityEngine;
using VContainer;

namespace Scene.Play
{
    public class ScorePresenter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        private ScoreManager _scoreManager;

        private int _currentScore = 0;
        private Coroutine _scoreRoutine;

        [Inject]
        public void Construct(ScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
            _scoreManager.OnScoreChanged += OnScoreChanged;
        }

        private void Start()
        {
            _currentScore = _scoreManager.TotalScore;
            _scoreText.text = _currentScore.ToString();
        }

        private void OnScoreChanged(int newScore)
        {
            if (_scoreRoutine != null)
            {
                StopCoroutine(_scoreRoutine);
            }
            _scoreRoutine = StartCoroutine(AnimateScoreChange(newScore));
        }

        private IEnumerator AnimateScoreChange(int targetScore)
        {
            float displayedScore = _currentScore;

            while (Mathf.Abs(displayedScore - targetScore) > 0.1f)
            {
                float diff = Mathf.Abs(displayedScore - targetScore);
                float speed = Mathf.Lerp(30f, 300f, diff / 100f); // 차이에 따라 속도 변화
                displayedScore = Mathf.MoveTowards(displayedScore, targetScore, speed * Time.deltaTime);
                _scoreText.text = Mathf.RoundToInt(displayedScore).ToString();
                yield return null;
            }

            _currentScore = targetScore;
            _scoreText.text = _currentScore.ToString();
        }

        private void OnDestroy()
        {
            _scoreManager.OnScoreChanged -= OnScoreChanged;
        }
    }
}