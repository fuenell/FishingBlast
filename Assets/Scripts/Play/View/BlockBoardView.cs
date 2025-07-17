using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scene.Play
{
    public class BlockBoardView : MonoBehaviour
    {
        private BlockCellView[,] _blockGrid = new BlockCellView[BoardConfig.Width, BoardConfig.Height];

        [SerializeField] private GameObject _blockCellViewPrefab;
        [SerializeField] private Transform _boardLeftBottom;
        [SerializeField] private Transform _boardRightTop;

        private Vector2 _centerPosition;

        private float _blockWidth;
        private float _blockHeight;

        public void Awake()
        {
            // Todo: 보드 위치가 바뀌면 갱신 필요 (해상도 동적 갱신 필요)
            _centerPosition = (_boardLeftBottom.position + _boardRightTop.position) * 0.5f;
            _blockWidth = (_boardRightTop.position.x - _boardLeftBottom.position.x) / (BoardConfig.Width - 1);
            _blockHeight = (_boardRightTop.position.y - _boardLeftBottom.position.y) / (BoardConfig.Height - 1);
        }

        public void PlaceBlock(BlockView block, Vector2Int cellPos)
        {
            foreach (Vector2Int blockPos in block.Model.GetShape())
            {
                Vector2Int placeCellPos = blockPos + cellPos;
                Vector3 worldPosition = CellToWorld(placeCellPos);
                BlockCellView blockCellView = Instantiate(_blockCellViewPrefab, worldPosition, Quaternion.identity, this.transform).GetComponent<BlockCellView>();
                _blockGrid[placeCellPos.x, placeCellPos.y] = blockCellView;
            }
        }

        private Vector2 CellToWorld(Vector2Int placeCellPos)
        {
            float x = _boardLeftBottom.position.x + placeCellPos.x * _blockWidth;
            float y = _boardLeftBottom.position.y + placeCellPos.y * _blockHeight;
            return new Vector2(x, y);
        }

        public Vector2Int WorldToCell(Vector3 position)
        {
            Vector2 boardPostion = position - _boardLeftBottom.position;
            int x = Mathf.RoundToInt(boardPostion.x / _blockWidth);
            int y = Mathf.RoundToInt(boardPostion.y / _blockHeight);
            return new Vector2Int(x, y);
        }

        public async UniTask ClearMatches(MatchedResult matches)
        {
            await UniTask.WaitForSeconds(0.2f);

            int width = _blockGrid.GetLength(0);
            int height = _blockGrid.GetLength(1);

            // 가로 줄 제거
            foreach (int y in matches.Rows)
            {
                for (int x = 0; x < width; x++)
                {
                    // 블럭이 존재할 때만 제거
                    if (_blockGrid[x, y] != null)
                    {
                        Destroy(_blockGrid[x, y].gameObject);
                        _blockGrid[x, y] = null; // 해당 위치를 비우기
                    }
                }
            }

            // 세로 줄 제거
            foreach (int x in matches.Columns)
            {
                for (int y = 0; y < height; y++)
                {
                    // 블럭이 존재할 때만 제거
                    if (_blockGrid[x, y] != null)
                    {
                        Destroy(_blockGrid[x, y].gameObject);
                        _blockGrid[x, y] = null; // 해당 위치를 비우기
                    }
                }
            }

        }
    }
}