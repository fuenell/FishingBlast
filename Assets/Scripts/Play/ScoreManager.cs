using System;

namespace Scene.Play
{
    public class ScoreManager
    {
        public int TotalScore { get; private set; }

        // 점수 변경 시 이벤트
        public event Action<int> OnScoreChanged;

        public void Add(int score)
        {
            if (score <= 0)
            {
                return;
            }

            TotalScore += score;
            OnScoreChanged?.Invoke(TotalScore);
        }

        public void Reset()
        {
            TotalScore = 0;
            OnScoreChanged?.Invoke(TotalScore);
        }

        public void AddMatchScore(MatchedResult matchedResult)
        {
            Add((matchedResult.Rows.Count + matchedResult.Columns.Count) * 100);
        }

        public void AddPlaceScore(BlockModel placedBlock)
        {
            Add(placedBlock.GetShape().Length);
        }
    }
}
