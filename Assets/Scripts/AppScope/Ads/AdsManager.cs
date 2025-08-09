using Cysharp.Threading.Tasks;

namespace FishingBlast.AppScope
{
    public interface IAdsService
    {
        public bool IsInitialized { get; }
        UniTask InitializeAsync();


        void LoadBannerAd();
        void DestroyBannerAd();

        bool CanShowRewardedAd { get; }
        UniTask<bool> ShowRewardedAd();
    }

    public class AdsManager
    {
        private IAdsService _adsService = null;

        public async UniTask InitializeAsync()
        {
            if (_adsService == null)
            {
                _adsService = new GoogleAdmobService();

                await _adsService.InitializeAsync();
            }
        }

        public void LoadBannerAd()
        {
            _adsService.LoadBannerAd();
        }

        public void DestroyBannerAd()
        {
            _adsService.DestroyBannerAd();
        }

        public bool CanShowRewardedAd()
        {
            return _adsService.CanShowRewardedAd;
        }

        public async UniTask<bool> ShowRewardedAd()
        {
            bool adShown = false;

            if (_adsService.CanShowRewardedAd)
            {
                adShown = await _adsService.ShowRewardedAd();
            }
            // 광고를 보여줄 수 없는 상태인 경우
            else
            {
                //ToastMessageManager.Instance.ShowToastMessage(TextTableKey.Ads_NoAd);
            }

            return adShown;
        }
    }
}