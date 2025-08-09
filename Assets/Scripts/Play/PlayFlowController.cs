using Cysharp.Threading.Tasks;
using FishingBlast.AppScope;
using FishingBlast.Data;
using FishingBlast.Interfaces;
using FishingBlast.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace FishingBlast.Play
{
    public class PlayFlowController : IStartable, IDisposable, IBackButtonHandler
    {
        private readonly SceneLoader _sceneLoader;
        private readonly AdsManager _adsManager;
        private readonly BlockQueuePresenter _blockQueuePresenter;
        private readonly BlockGenerator _blockGenerator;
        private readonly BlockBoard _blockBoard;
        private readonly BlockBoardView _blockBoardView;
        private readonly BlockDragController _blockDragController;
        private readonly ScoreManager _scoreManager;
        private readonly DataManager _dataManager;
        private readonly PopupManager _popupManager;

        public PlayFlowController(
            SceneLoader sceneLoader,
            AdsManager adsManager,
            BlockQueuePresenter blockQueuePresenter,
            BlockGenerator blockGenerator,
            BlockBoard blockBoard,
            BlockBoardView blockBoardView,
            BlockDragController blockDragController,
            ScoreManager scoreManager,
            DataManager dataManager,
            PopupManager popupManager)
        {
            _sceneLoader = sceneLoader;
            _adsManager = adsManager;
            _blockQueuePresenter = blockQueuePresenter;
            _blockGenerator = blockGenerator;
            _blockBoard = blockBoard;
            _blockBoardView = blockBoardView;
            _blockDragController = blockDragController;
            _scoreManager = scoreManager;
            _dataManager = dataManager;
            _popupManager = popupManager;
        }


        public void Start()
        {
            _popupManager.SetBackButtonHandler(this);

            _adsManager.LoadBannerAd(); // 게임 시작 시 배너 광고 로드

            LoadGame(_dataManager.GetPlayerData());

            StartGameLoop().Forget();
        }

        public void Dispose()
        {
            _popupManager.SetBackButtonHandler(null);
            _adsManager.DestroyBannerAd();
        }

        // 게임 중 Back 버튼이 눌렸을 때 호출되는 메서드
        public void HandleBackButtonOnTop()
        {
            _popupManager.Show<QuitPopup>();
        }

        private void LoadGame(PlayerData playerData)
        {
            // 첫판 판정
            if (playerData.NeedTutorial)
            {
                // Todo: 튜토리얼 세팅 및 실행
                return;
            }

            // 이전 게임이 진행 중이면 복구
            if (playerData.IsPlaying)
            {
                _scoreManager.SetScore(playerData.NowScore);
                _blockQueuePresenter.CreateAndShowBlocks(playerData.BlockQueue);
                _blockBoard.SetBoard(playerData.GameBoard);
                _blockBoardView.SetBoard(_blockBoard.Grid);
            }
        }

        private async UniTaskVoid StartGameLoop()
        {
            Debug.Log("Game Start");
            bool isGameOver = false;

            while (isGameOver == false)
            {
                // 큐의 블럭을 모두 사용하면 다시 생성
                if (_blockQueuePresenter.RemainBlockIsZero())
                {
                    List<BlockModel> blocks = _blockGenerator.GenerateNextBlocks();
                    _blockQueuePresenter.CreateAndShowBlocks(blocks);
                }

                //// Todo: 불가능 시 패배 처리
                if (_blockBoard.IsGameOver(_blockQueuePresenter.RemainBlockList))
                {
                    Debug.Log("Game Over");
                    isGameOver = true;
                    _dataManager.ResetGame();
                    _dataManager.SaveData().Forget();
                    break;
                }

                BlockModel placedBlock = await _blockDragController.DragBlock();

                // 블럭을 유효한 곳에 배치함
                if (placedBlock != null)
                {
                    _blockQueuePresenter.RemoveBlock(placedBlock);
                    _scoreManager.AddPlaceScore(placedBlock);   // 블럭 배치 점수 추가

                    // 매치 성공 블럭 삭제 및 삭제 연출
                    MatchedResult matchedResult = _blockBoard.GetMatchedLines(); // 매치 성공한 라인 목록 반환
                    if (matchedResult.IsEmpty == false)
                    {
                        _blockBoard.ClearMatches(matchedResult);    // 매치 성공한 줄 제거
                        _scoreManager.AddMatchScore(matchedResult);     // 매치 점수 지급

                        _dataManager.UpdateGame(_blockBoard.Grid, _blockQueuePresenter.RemainBlockList, _scoreManager.TotalScore);
                        _dataManager.AddNewFish(0, 1.0f);
                        _dataManager.SaveData().Forget();

                        await _blockBoardView.ClearMatches(matchedResult); // 이펙트 및 오브젝트 삭제
                    }
                    else
                    {
                        _dataManager.UpdateGame(_blockBoard.Grid, _blockQueuePresenter.RemainBlockList, _scoreManager.TotalScore);
                        _dataManager.SaveData().Forget();
                    }

                }

                // Todo: 점수에 따른 물고기 획득 연출

                //await UniTask.Delay(500); // 잠깐 대기 후 다음 루프
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