using UnityEngine;

namespace Scene.Play
{
    public class BlockBoardView : MonoBehaviour
    {
        [SerializeField] private Transform _boardLeftBottom;
        [SerializeField] private Transform _boardRightTop;

        private Vector2 _centerPosition;

        private float _blockWidth;
        private float _blockHeight;

        public void Awake()
        {
            _centerPosition = (_boardLeftBottom.position + _boardRightTop.position) * 0.5f;
            _blockWidth = (_boardRightTop.position.x - _boardLeftBottom.position.x) / (BoardConfig.Width - 1);
            _blockHeight = (_boardRightTop.position.y - _boardLeftBottom.position.y) / (BoardConfig.Height - 1);
        }

        public void PlaceBlock(BlockView draggingBlock, Vector2Int cellPos)
        {

        }

        public Vector2Int WorldToCell(Vector3 position)
        {
            Vector2 boardPostion = position - _boardLeftBottom.position;
            int x = Mathf.RoundToInt(boardPostion.x / _blockWidth);
            int y = Mathf.RoundToInt(boardPostion.y / _blockHeight);
            return new Vector2Int(x, y);
        }
    }
}