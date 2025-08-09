using UnityEngine;
using UnityEngine.UI;

namespace FishingBlast.Utilities
{
    public class PuzzleLayoutFitter : MonoBehaviour
    {
        private const float TopUIHeight = 720;
        private const float BannerHeight = 150;

        [SerializeField]
        private CanvasScaler _canvasScaler;

        [SerializeField]
        private RectTransform _safeAreaRect;

        private Vector2Int _lastScreenSize;
        private Vector2 _lastSafeAreaSize;

        private void Start()
        {
            UpdatePuzzleTransform();
        }

        private void Update()
        {
            if (IsChangedScreenSize() || IsChangedSafeAreaSize())
            {
                UpdatePuzzleTransform();
            }
        }

        private bool IsChangedSafeAreaSize()
        {
            return _lastSafeAreaSize != _safeAreaRect.rect.size;
        }

        private bool IsChangedScreenSize()
        {
            return Screen.width != _lastScreenSize.x || Screen.height != _lastScreenSize.y;
        }

        private void UpdatePuzzleTransform()
        {
            _lastScreenSize = new Vector2Int(Screen.width, Screen.height);
            _lastSafeAreaSize = _safeAreaRect.rect.size;

            // safe area 범위 중 UI를 제외한 중앙에 배치
            this.transform.position = CalculateGameAreaPosition();

            // 퍼즐 크기 조정
            this.transform.localScale = GetPuzzleScale();
        }

        private Vector3 GetPuzzleScale()
        {
            Vector3 puzzleScale = Vector3.one;

            // target 해상도보다 세로 비율이 길어지면 그만큼 퍼즐 크기를 줄인다
            float screenRatio = (float)Screen.width / Screen.height;
            float referenceRatio = _canvasScaler.referenceResolution.x / _canvasScaler.referenceResolution.y;
            if (screenRatio < referenceRatio)
            {
                float newSize = screenRatio / referenceRatio;
                puzzleScale *= newSize;
            }

            // safe area 범위 중 UI를 제외한 중앙에 퍼즐이 들어갈 수 있도록 크기 조정
            float needHeight = _canvasScaler.referenceResolution.y - BannerHeight - TopUIHeight;
            float nowHeight = _safeAreaRect.rect.size.y - BannerHeight - TopUIHeight;
            if (nowHeight < needHeight)
            {
                float newSize = nowHeight / needHeight;
                puzzleScale *= newSize;
            }

            return puzzleScale;
        }

        // safe area 범위 중 UI를 제외한 중앙에 배치
        public Vector2 CalculateGameAreaPosition()
        {
            Vector2 centerPosition = _safeAreaRect.position;

            float canvasToWorldScale = 0.01f;

            // 목표 해상도보다 세로 비율이 길어지면 그만큼 UI의 실제 크기가 줄어들기 때문에 보정
            float screenRatio = (float)Screen.width / Screen.height;
            float referenceRatio = _canvasScaler.referenceResolution.x / _canvasScaler.referenceResolution.y;
            if (screenRatio < referenceRatio)
            {
                canvasToWorldScale *= screenRatio / referenceRatio;
            }

            // UI 높이 제외
            centerPosition.y += (BannerHeight * canvasToWorldScale * 0.5f) - (TopUIHeight * canvasToWorldScale * 0.5f);
            return centerPosition;
        }
    }
}