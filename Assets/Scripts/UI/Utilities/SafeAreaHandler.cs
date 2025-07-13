using UnityEngine;

namespace UI.Utilities
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaHandler : MonoBehaviour
    {
        RectTransform panel;

        void Awake()
        {
            panel = GetComponent<RectTransform>();
            ApplySafeArea();
        }

        void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            // 스크린 좌표를 0~1 사이 비율 좌표로 변환
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            panel.anchorMin = anchorMin;
            panel.anchorMax = anchorMax;
        }
    }
}