using AppScope.Core;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace Scene.Play
{
    public class PlayFlowController : IStartable
    {
        private readonly SceneLoader _sceneLoader;
        private readonly AdsManager _adsManager;
        private readonly BlockQueuePresenter _blockQueuePresenter;
        private readonly BlockGenerator _blockGenerator;
        private readonly BlockBoard _blockBoard;
        private readonly BlockBoardView _blockBoardView;
        private readonly BlockDragController _blockDragController;
        private readonly ScoreManager _scoreManager;

        public PlayFlowController(SceneLoader sceneLoader, AdsManager adsManager, BlockQueuePresenter blockQueuePresenter, BlockGenerator blockGenerator, BlockBoard blockBoard, BlockBoardView blockBoardView, BlockDragController blockDragController, ScoreManager scoreManager)
        {
            _sceneLoader = sceneLoader;
            _adsManager = adsManager;
            _blockQueuePresenter = blockQueuePresenter;
            _blockGenerator = blockGenerator;
            _blockBoard = blockBoard;
            _blockBoardView = blockBoardView;
            _blockDragController = blockDragController;
            _scoreManager = scoreManager;
        }

        public void Start()
        {
            StartGameLoop().Forget();
        }

        private async UniTaskVoid StartGameLoop()
        {
            _adsManager.LoadBannerAd(); // 게임 시작 시 배너 광고 로드

            Debug.Log("Game Start");
            bool isGameOver = false;

            while (isGameOver == false)
            {
                List<BlockModel> blocks = _blockGenerator.GenerateNextBlocks();
                _blockQueuePresenter.CreateAndShowBlocks(blocks);

                while (_blockQueuePresenter.AreAllBlocksPlaced == false)
                {
                    //// Todo: 불가능 시 패배 처리
                    if (_blockBoard.IsGameOver(_blockQueuePresenter.RemainBlockList))
                    {
                        Debug.Log("Game Over");
                        isGameOver = true;
                        break;
                    }

                    BlockModel placedBlock = await _blockDragController.DragBlock();

                    if (placedBlock != null)
                    {
                        _blockQueuePresenter.RemoveBlock(placedBlock);
                        _scoreManager.AddPlaceScore(placedBlock);   // 블럭 배치 점수 추가
                    }

                    // 매치 성공 블럭 삭제 및 삭제 연출
                    MatchedResult matchedResult = _blockBoard.GetMatchedLines(); // 매치 성공한 라인 목록 반환
                    if (matchedResult.IsEmpty == false)
                    {
                        _blockBoard.ClearMatches(matchedResult);    // 매치 성공한 줄 제거
                        _scoreManager.AddMatchScore(matchedResult);     // 매치 점수 지급
                        await _blockBoardView.ClearMatches(matchedResult); // 이펙트 및 오브젝트 삭제
                    }

                    // Todo: 점수에 따른 물고기 획득 연출
                }


                await UniTask.Delay(500); // 잠깐 대기 후 다음 루프
            }

            // Todo: 게임 오버 연출

            await UniTask.Delay(500); // 잠깐 대기

            Debug.Log("Game End");
            _adsManager.DestroyBannerAd(); // 게임 종료 시 배너 광고 제거

            // Todo: 게임 오버 후 광고 보여주기

            _sceneLoader.LoadSceneAsync("Title").Forget();
        }
    }
}