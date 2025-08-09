using UnityEngine;
using UnityEngine.UI;

namespace FishingBlast.Utilities
{
    public class AdMobBannerSizeCalculator : MonoBehaviour
    {
        // AdMob 기본 배너 크기 (dp)
        private const float AdMobBannerWidthDp = 320f;
        private const float AdMobBannerHeightDp = 50f;

        public RectTransform test;
        private CanvasScaler canvasScaler;

        void Start()
        {
            Vector2 bannerSize = new Vector2(300, GetAdMobBannerPixelSize());
            Debug.Log($"AdMob 배너 실제 크기 (px): {bannerSize.x} x {bannerSize.y}");
            //bannerSize += Vector2.one * 5;
            test.sizeDelta = bannerSize;
        }

        public float GetAdMobBannerPixelSize()
        {
            const float bannerDpHeight = 50f;

            float screenHeight = Screen.height;
            float referenceHeight = canvasScaler.referenceResolution.y;

            // Canvas의 스케일 방식이 Scale With Screen Size일 때만 유효
            if (canvasScaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                Debug.LogWarning("CanvasScaler가 ScaleWithScreenSize 모드가 아닙니다.");
                return bannerDpHeight;
            }

            float heightInCanvasUnits = bannerDpHeight * (referenceHeight / screenHeight);
            return heightInCanvasUnits;
        }
    }
}
