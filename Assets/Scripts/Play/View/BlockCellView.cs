using UnityEngine;

namespace Scene.Play
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BlockCellView : MonoBehaviour
    {
        [SerializeField] private Sprite[] _colorSprites;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetColor(int colorIndex)
        {
            if (0 <= colorIndex && colorIndex < _colorSprites.Length)
            {
                _spriteRenderer.sprite = _colorSprites[colorIndex];
            }
        }
    }
}