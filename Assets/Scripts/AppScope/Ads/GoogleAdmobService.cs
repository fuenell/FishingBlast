using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;
using UnityEngine;

namespace FishingBlast.AppScope
{
    public class GoogleAdmobService : IAdsService
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        private const string RewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE && !UNITY_EDITOR
        private const string RewardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        private const string RewardedAdUnitId = "unused";
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
    private const string BannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE && !UNITY_EDITOR
    private const string BannerAdUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        private const string BannerAdUnitId = "unused";
#endif

        public bool IsInitialized { get; private set; } = false;
        private bool _isInitializing = false;

        private RewardedAd _rewardedAd;

        private bool _isLoadingRewaredAd = false;

        private float _loadRetrySeconds = 10f;
        private int _maxLoadRetryCount = 3;

        public bool CanShowRewardedAd => _rewardedAd != null && _rewardedAd.CanShowAd();

        public async UniTask InitializeAsync()
        {
            if (IsInitialized || _isInitializing)
                return;

            _isInitializing = true;

            var tcs = new UniTaskCompletionSource();

            MobileAds.Initialize(_ =>
            {
                tcs.TrySetResult();
            });

            await tcs.Task;

            IsInitialized = true;
            _isInitializing = false;

            LoadRewardedAd();
        }

        private async void LoadRewardedAd()
        {
            if (_isLoadingRewaredAd)
            {
                return;
            }

            _isLoadingRewaredAd = true;

            if (!CanShowRewardedAd)
            {
                DestroyAd(ref _rewardedAd);
            }

            _rewardedAd = await LoadAdWithRetry(RewardedAdUnitId, "일반");

            _isLoadingRewaredAd = false;
        }

        // 광고 로드 메서드
        private async UniTask<RewardedAd> LoadAdWithRetry(string adUnitId, string logPrefix)
        {
            for (int retryCount = 0; retryCount < _maxLoadRetryCount; retryCount++)
            {
                var tcs = new UniTaskCompletionSource<(RewardedAd, LoadAdError)>();
                var adRequest = new AdRequest();

                RewardedAd.Load(adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
                {
                    tcs.TrySetResult((ad, error));
                });

                (RewardedAd loadedAd, LoadAdError loadError) = await tcs.Task;

                if (loadError != null)
                {
                    Debug.LogError($"{logPrefix} 광고 로드 실패/{retryCount}/{loadError.GetMessage()}");
                    await UniTask.WaitForSeconds(_loadRetrySeconds);
                }
                else if (loadedAd == null)
                {
                    Debug.LogError($"{logPrefix} 알 수 없는 이유로 광고 로드 실패/{retryCount}");
                    await UniTask.WaitForSeconds(_loadRetrySeconds);
                }
                else
                {
                    Debug.Log($"{logPrefix} 광고 로드 성공/{retryCount}");
                    return loadedAd;
                }
            }

            return null;
        }

        public async UniTask<bool> ShowRewardedAd()
        {
            var tcs = new UniTaskCompletionSource<Reward>();
            bool adShown = false;

            if (CanShowRewardedAd)
            {
                _rewardedAd.Show(reward => tcs.TrySetResult(reward));

                Reward reward = await tcs.Task;

                if (reward != null && 0 < reward.Amount)
                {
                    adShown = true;
                }
            }

            LoadRewardedAd();

            return adShown;
        }

        private void DestroyAd(ref RewardedAd rewardedAd)
        {
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }
        }

        BannerView _bannerView;

        private void CreateBannerView()
        {
            Debug.Log("Creating banner view");

            // If we already have a banner, destroy the old one.
            if (_bannerView != null)
            {
                DestroyBannerAd();
            }

            // Create a 320x50 banner at top of the screen
            _bannerView = new BannerView(BannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
        }

        /// <summary>
        /// Creates the banner view and loads a banner ad.
        /// </summary>
        public void LoadBannerAd()
        {
            // create an instance of a banner view first.
            if (_bannerView == null)
            {
                CreateBannerView();
            }

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            Debug.Log("Loading banner ad.");
            _bannerView.LoadAd(adRequest);
        }

        /// <summary>
        /// Destroys the banner view.
        /// </summary>
        public void DestroyBannerAd()
        {
            if (_bannerView != null)
            {
                Debug.Log("Destroying banner view.");
                _bannerView.Destroy();
                _bannerView = null;
            }
        }
    }
}