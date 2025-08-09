using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace FishingBlast.Play
{
    public class BlockBoardView : MonoBehaviour
    {
        private BlockCellView[,] _blockGrid = new BlockCellView[BoardConfig.Width, BoardConfig.Height];

        [SerializeField] private GameObject _blockCellViewPrefab;
        [SerializeField] private Transform _boardLeftBottom;
        [SerializeField] private Transform _boardRightTop;

        private CameraShaker _cameraShaker;

        private float _localBlockWidth;
        private float _localBlockHeight;

        private float _worldBlockWidth;
        private float _worldBlockHeight;

        private Vector3 _lastLeftBottomPos;
        private Vector3 _lastRightTopPos;

        [Inject]
        public void Construct(CameraShaker cameraShaker)
        {
            _cameraShaker = cameraShaker;
            _localBlockWidth = (_boardRightTop.localPosition.x - _boardLeftBottom.localPosition.x) / (BoardConfig.Width - 1);
            _localBlockHeight = (_boardRightTop.localPosition.y - _boardLeftBottom.localPosition.y) / (BoardConfig.Height - 1);

            UpdateWorldBlockSizeCache();
        }

        public void SetBoard(int[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != BlockBoard.EmptyNum)
                    {
                        PlcaeCell(grid[i, j], new Vector2Int(i, j));
                    }
                }
            }
        }

        public void Start()
        {
        }

        private void Update()
        {
            if (HasBoardPositionChanged())
            {
                UpdateWorldBlockSizeCache();
            }
        }

        private bool HasBoardPositionChanged()
        {
            return _boardLeftBottom.position != _lastLeftBottomPos || _boardRightTop.position != _lastRightTopPos;
        }

        private void UpdateWorldBlockSizeCache()
        {
            _lastLeftBottomPos = _boardLeftBottom.position;
            _lastRightTopPos = _boardRightTop.position;

            _worldBlockWidth = (_boardRightTop.position.x - _boardLeftBottom.position.x) / (BoardConfig.Width - 1);
            _worldBlockHeight = (_boardRightTop.position.y - _boardLeftBottom.position.y) / (BoardConfig.Height - 1);
        }

        public void PlaceBlock(BlockView block, Vector2Int gridPosition)
        {
            foreach (Vector2Int blockPos in block.Model.Shape)
            {
                Vector2Int placeCellPos = blockPos + gridPosition;
                PlcaeCell(block.ColorIndex, placeCellPos);
            }
        }

        private void PlcaeCell(int colorIndex, Vector2Int gridPosition)
        {
            Vector3 boardPosition = CellToBoard(gridPosition);
            BlockCellView blockCellView = Instantiate(_blockCellViewPrefab, this.transform).GetComponent<BlockCellView>();
            blockCellView.SetColor(colorIndex);
            blockCellView.transform.localPosition = boardPosition;
            _blockGrid[gridPosition.x, gridPosition.y] = blockCellView;
        }

        public Vector2 CellToBoard(Vector2Int gridPosition)
        {
            float x = _boardLeftBottom.localPosition.x + gridPosition.x * _localBlockWidth;
            float y = _boardLeftBottom.localPosition.y + gridPosition.y * _localBlockHeight;
            return new Vector2(x, y);
        }

        public Vector2Int WorldToGrid(Vector3 worldPosition)
        {
            Vector2 boardPostion = worldPosition - _boardLeftBottom.position;
            int x = Mathf.RoundToInt(boardPostion.x / _worldBlockWidth);
            int y = Mathf.RoundToInt(boardPostion.y / _worldBlockHeight);
            return new Vector2Int(x, y);
        }

        public async UniTask ClearMatches(MatchedResult matches)
        {
            int width = _blockGrid.GetLength(0);
            int height = _blockGrid.GetLength(1);

            _cameraShaker.Shake(0.1f, (matches.Rows.Count + matches.Columns.Count) * 0.1f);

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

            //await UniTask.WaitForSeconds(0.2f);
        }
    }
}