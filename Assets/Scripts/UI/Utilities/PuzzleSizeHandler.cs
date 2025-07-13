using System.Collections;
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
            // safe area의 높이가
            Vector2 gameAreaPosition = GameAreaPosition();
            gameAreaPosition.y += _yOffset;
            this.transform.position = gameAreaPosition;


            // target 해상도보다 세로 비율이 길어지면 그만큼 퍼즐 크기를 줄인다
            float safeAreaScreenRatio = _safeAreaRect.rect.size.x / _safeAreaRect.rect.size.y;
            float referenceRatio = _canvasScaler.referenceResolution.x / _canvasScaler.referenceResolution.y;


            if (_safeAreaRect.rect.size.y < _canvasScaler.referenceResolution.y)
            {
                float newSize = _safeAreaRect.rect.size.y / _canvasScaler.referenceResolution.y;
                print(newSize);
                this.transform.localScale = new Vector3(newSize, newSize, 1);
            }
            else if (safeAreaScreenRatio < referenceRatio)
            {
                float newSize = safeAreaScreenRatio / referenceRatio;
                this.transform.localScale = new Vector3(newSize, newSize, 1);
            }
            else
            {
                this.transform.localScale = Vector3.one;
            }
        }

        public Vector2 GameAreaPosition()
        {
            Vector2 position = _safeAreaRect.position;
            position.y += (BannerHeight * 0.005f) - (TopUIHeight * 0.005f);
            return position;
        }
    }
}