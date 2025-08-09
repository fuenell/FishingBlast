using UnityEngine;

namespace FishingBlast.AppScope
{
    public class GameInitializer
    {
        public bool IsInitialized { get; private set; } = false;

        private readonly AdsManager _adsManager;

        public GameInitializer(AdsManager adsManager)
        {
            _adsManager = adsManager;

            Init();
        }

        public async void Init()
        {
            Application.targetFrameRate = 60;

            await _adsManager.InitializeAsync();

            Debug.Log("GameInitializer 초기화 완료");

            IsInitialized = true;
        }
    }
}