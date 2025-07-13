using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace Scene.Play
{
    public class PlayFlowController : IStartable
    {
        private readonly BlockPresenter _blockPresenter;
        private readonly BlockGenerator _blockGenerator;
        private readonly BlockBoard _blockBoard;

        public PlayFlowController(BlockPresenter blockPresenter, BlockGenerator blockGenerator, BlockBoard blockBoard)
        {
            _blockPresenter = blockPresenter;
            _blockGenerator = blockGenerator;
            _blockBoard = blockBoard;
        }

        public void Start()
        {
            StartGameLoop().Forget();
        }

        private async UniTaskVoid StartGameLoop()
        {
            while (true)
            {
                var blocks = _blockGenerator.GenerateNextBlocks();
                _blockPresenter.ShowBlocks(blocks);

                await UniTask.WaitUntil(() => _blockPresenter.AreAllBlocksPlaced); // 유저가 모든 블럭을 배치할 때까지 대기

                _blockBoard.CheckMatches(); // 줄이 완성되었는지 검사

                if (_blockBoard.IsGameOver(blocks))
                {
                    Debug.Log("Game Over");
                    break;
                }

                await UniTask.Delay(500); // 잠깐 대기 후 다음 루프
            }
        }
    }
}