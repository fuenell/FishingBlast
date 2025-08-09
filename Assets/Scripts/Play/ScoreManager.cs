using FishingBlast.Data;
using System;

namespace FishingBlast.Play
{
    public class ScoreManager
    {
        public int TotalScore { get; private set; }

        // 점수 변경 시 이벤트
        public event Action<int> OnScoreChanged;

        public void SetScore(int newScore)
        {
            TotalScore = newScore;
            //OnScoreChanged?.Invoke(TotalScore);
        }

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
            Add(placedBlock.Shape.Length);
        }
    }
}
