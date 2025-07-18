using UnityEngine;
using UnityEngine.UI;

namespace UI.Utilities
{
    public class PuzzleSizeHandler : MonoBehaviour
    {
        [SerializeField]
        private CanvasScaler _canvasScaler;

        [SerializeField]
        private RectTransform _safeAreaRect;

        [SerializeField]
        private float _yOffset = 1.41f;


        private float TopUIHeight = 720;
        private float BannerHeight = 150;

        private void Start()
        {
            SetPuzzleSize();
        }

#if UNITY_EDITOR
        private void Update()
        {
            SetPuzzleSize();
        }
#endif

        private void SetPuzzleSize()
        {
            this.transform.localScale = Vector3.one;

            // safe area의 높이가
            Vector2 gameAreaPosition = GameAreaPosition();
            //gameAreaPosition.y += _yOffset;
            this.transform.position = gameAreaPosition;

            // target 해상도보다 세로 비율이 길어지면 그만큼 퍼즐 크기를 줄인다
            float screenRatio = (float)Screen.width / Screen.height;
            float referenceRatio = _canvasScaler.referenceResolution.x / _canvasScaler.referenceResolution.y;
            if (screenRatio < referenceRatio)
            {
                float newSize = screenRatio / referenceRatio;
                this.transform.localScale = new Vector3(newSize, newSize, 1);
            }

            float needHeight = _canvasScaler.referenceResolution.y - BannerHeight - TopUIHeight;
            float nowHeight = _safeAreaRect.rect.size.y - BannerHeight - TopUIHeight;

            if (nowHeight < needHeight)
            {
                float newSize = nowHeight / needHeight;
                this.transform.localScale *= newSize;
            }
        }

        public Vector2 GameAreaPosition()
        {
            Vector2 position = _safeAreaRect.position;

            float screenToWorldRatio = 0.01f;

            float screenRatio = (float)Screen.width / Screen.height;
            float referenceRatio = _canvasScaler.referenceResolution.x / _canvasScaler.referenceResolution.y;
            if (screenRatio < referenceRatio)
            {
                screenToWorldRatio *= screenRatio / referenceRatio;
            }

            position.y += (BannerHeight * screenToWorldRatio * 0.5f) - (TopUIHeight * screenToWorldRatio * 0.5f);
            return position;
        }
    }
}