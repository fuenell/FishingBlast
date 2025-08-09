namespace FishingBlast.Play
{
    using System.Collections;
    using UnityEngine;

    public class CameraShaker : MonoBehaviour
    {
        private Vector3 _originalPos;
        private Coroutine _shakeCoroutine;

        public void Shake(float duration, float magnitude)
        {
            if (_shakeCoroutine != null)
                StopCoroutine(_shakeCoroutine);

            _shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
        }

        private IEnumerator ShakeCoroutine(float duration, float magnitude)
        {
            _originalPos = transform.localPosition;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;
                transform.localPosition = _originalPos + new Vector3(x, y, 0);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = _originalPos;
        }
    }

}
