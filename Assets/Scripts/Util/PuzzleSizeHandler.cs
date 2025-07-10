using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleSizeHandler : MonoBehaviour
{
    [SerializeField]
    private CanvasScaler _canvasScaler;

    [SerializeField]
    private RectTransform _safeAreaRect;

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
        this.transform.position = new Vector3(_safeAreaRect.position.x, _safeAreaRect.position.y, 0);


        // target 해상도보다 세로 비율이 길어지면 그만큼 퍼즐 크기를 줄인다
        float safeAreaScreenRatio = _safeAreaRect.rect.size.x / _safeAreaRect.rect.size.y;
        float referenceRatio = _canvasScaler.referenceResolution.x / _canvasScaler.referenceResolution.y;

        if (safeAreaScreenRatio < referenceRatio)
        {
            float newSize = safeAreaScreenRatio / referenceRatio;
            this.transform.localScale = new Vector3(newSize, newSize, 1);
        }
        else
        {
            this.transform.localScale = Vector3.one;
        }
    }
}
