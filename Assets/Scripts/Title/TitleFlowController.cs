using Cysharp.Threading.Tasks;
using FishingBlast.AppScope;
using FishingBlast.Interfaces;
using FishingBlast.UI;
using System;
using VContainer.Unity;

namespace FishingBlast.Title
{
    public class TitleFlowController : IStartable, IDisposable, IBackButtonHandler
    {
        private readonly SceneLoader _sceneLoader;
        private readonly PopupManager _popupManager;
        private bool _isNavigating = false;

        public TitleFlowController(SceneLoader sceneLoader, PopupManager popupManager)
        {
            _sceneLoader = sceneLoader;
            _popupManager = popupManager;
        }

        public void Start()
        {
            _popupManager.SetBackButtonHandler(this);
        }

        public void Dispose()
        {
            _popupManager.SetBackButtonHandler(null);
        }

        // 게임 중 Back 버튼이 눌렸을 때 호출되는 메서드
        public void HandleBackButtonOnTop()
        {
            _popupManager.Show<QuitPopup>();
        }

        public async UniTaskVoid OnClickStartButton()
        {
            if (_isNavigating)
            {
                return;
            }
            _isNavigating = true;

            await _sceneLoader.LoadSceneAsync("Play");
        }
    }
}