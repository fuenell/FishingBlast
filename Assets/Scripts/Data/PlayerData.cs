namespace FishingBlast.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    // 모든 플레이어 데이터를 담는 통합 클래스
    [Serializable]
    public class PlayerData
    {
        // --- Private Fields with [SerializeField] ---
        // 모든 데이터 필드를 private으로 선언하고 [SerializeField]를 붙여
        // 캡슐화와 직렬화를 동시에 만족시킵니다.
        [SerializeField] private List<FishData> _caughtFishes = new List<FishData>();
        [SerializeField] private int _highScore;
        [SerializeField] private int _nowScore;
        [SerializeField] private int[] _gameBoard;
        [SerializeField] private List<BlockModel> _blockQueue = new List<BlockModel>();

        // --- Public Read-only Properties ---
        // 외부에서는 이 프로퍼티들을 통해 데이터를 '읽기만' 할 수 있습니다.
        public List<FishData> CaughtFishes => _caughtFishes;
        public int HighScore => _highScore;
        public int NowScore => _nowScore;
        public int[] GameBoard => _gameBoard;
        public List<BlockModel> BlockQueue => _blockQueue;

        public bool IsPlaying
        {
            get
            {
                return GameBoard != null && GameBoard.Length != 0;
            }
        }

        public bool NeedTutorial
        {
            get
            {
                return HighScore == 0;
            }
        }

        // --- Constructor ---
        public PlayerData()
        {
            // 기본값 설정
            _highScore = 0;
            _nowScore = 0;
        }

        // --- Public Methods for Modification ---
        // 외부에서 데이터를 '변경'하고 싶다면, 반드시 이 메소드들을 거쳐야 합니다.
        // 이를 통해 데이터 변경 로직을 한 곳에서 중앙 관리할 수 있습니다.
        public void ResetGame()
        {
            _gameBoard = null;
            _blockQueue = null;
            _nowScore = 0;
        }

        public void UpdateGameBoard(int[,] newBoard, List<BlockModel> newQueue)
        {
            // 2D 배열을 1D로 변환하는 로직
            int boardWidth = newBoard.GetLength(0);
            int boardHeight = newBoard.GetLength(1);
            _gameBoard = new int[boardWidth * boardHeight];
            for (int y = 0; y < boardHeight; y++)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    _gameBoard[y * boardWidth + x] = newBoard[x, y];
                }
            }

            _blockQueue = newQueue;
        }

        public void UpdateScore(int newScore)
        {
            _nowScore = newScore;
            if (_nowScore > _highScore)
            {
                _highScore = _nowScore;
            }
        }

        public void AddOrUpdateFish(int fishId, float size)
        {
            var fish = _caughtFishes.FirstOrDefault(f => f.Id == fishId);
            if (fish != null)
            {
                fish.AddNewRecord(size);
            }
            else
            {
                _caughtFishes.Add(new FishData(fishId, 1, size));
            }
        }
    }
}