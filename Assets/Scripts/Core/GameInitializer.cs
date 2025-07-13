using UnityEngine;
using VContainer.Unity;

namespace AppScope.Core
{
    public class GameInitializer
    {
        public bool IsInitialized { get; private set; } = false;

        private AdsManager _adsManager;

        public GameInitializer(AdsManager adsManager)
        {
            _adsManager = adsManager;

            Init();
        }

        public async void Init()
        {
            await _adsManager.InitializeAsync();

            Debug.Log("GameInitializer 초기화 완료");

            IsInitialized = true;
        }
    }
}