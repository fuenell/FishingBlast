using Cysharp.Threading.Tasks;

namespace AppScope.Core
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
        private IAdsService m_adsService = null;

        public async UniTask InitializeAsync()
        {
            if (m_adsService == null)
            {
                m_adsService = new GoogleAdmobService();

                await m_adsService.InitializeAsync();
            }
        }

        public void LoadBannerAd()
        {
            m_adsService.LoadBannerAd();
        }

        public void DestroyBannerAd()
        {
            m_adsService.DestroyBannerAd();
        }

        public bool CanShowRewardedAd()
        {
            return m_adsService.CanShowRewardedAd;
        }

        public async UniTask<bool> ShowRewardedAd()
        {
            bool adShown = false;

            if (m_adsService.CanShowRewardedAd)
            {
                adShown = await m_adsService.ShowRewardedAd();
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