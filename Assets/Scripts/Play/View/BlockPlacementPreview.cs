using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace FishingBlast.Play
{
    public class BlockPlacementPreview : MonoBehaviour
    {
        [SerializeField] private GameObject _blockCellPreviewPrefab;

        private BlockBoardView _blockBoardView;

        private List<BlockCellView> _blockCellPreviewList = new List<BlockCellView>();

        [Inject]
        public void Construct(BlockBoardView blockBoardView)
        {
            _blockBoardView = blockBoardView;
        }

        public void ShowPreview(BlockView block, Vector2Int gridPos, MatchedResult matches)
        {
            // 배치 위치 프리뷰 표시
            ShowBlockGhost(block, gridPos);

            // 매치 라인 하이라이트
            HighlightMatchedLines(block, matches);
        }

        public void HidePreview()
        {
            _blockCellPreviewList.ForEach(cell => cell.gameObject.SetActive(false));
        }

        private void ShowBlockGhost(BlockView block, Vector2Int cellPos)
        {
            int index = 0;

            foreach (Vector2Int blockPos in block.Model.Shape)
            {
                if (_blockCellPreviewList.Count <= index)
                {
                    BlockCellView blockCellPreview = Instantiate(_blockCellPreviewPrefab, this.transform).GetComponent<BlockCellView>();
                    _blockCellPreviewList.Add(blockCellPreview);
                }

                Vector2Int placeCellPos = blockPos + cellPos;
                Vector3 boardPosition = _blockBoardView.CellToBoard(placeCellPos);
                _blockCellPreviewList[index].SetColor(block.ColorIndex);
                _blockCellPreviewList[index].transform.localPosition = boardPosition;
                _blockCellPreviewList[index].gameObject.SetActive(true);
                index++;
            }

            for (; index < _blockCellPreviewList.Count; index++)
            {
                _blockCellPreviewList[index].gameObject.SetActive(false);
            }
        }

        private void HighlightMatchedLines(BlockView block, MatchedResult matches)
        {
            int width = BoardConfig.Width;
            int height = BoardConfig.Height;
            int index = _blockCellPreviewList.Count;

            // 가로 줄 강조
            foreach (int y in matches.Rows)
            {
                for (int x = 0; x < width; x++)
                {
                    if (_blockCellPreviewList.Count <= index)
                    {
                        var cell = Instantiate(_blockCellPreviewPrefab, this.transform).GetComponent<BlockCellView>();
                        _blockCellPreviewList.Add(cell);
                    }

                    Vector2Int pos = new Vector2Int(x, y);
                    Vector3 boardPos = _blockBoardView.CellToBoard(pos);
                    var previewCell = _blockCellPreviewList[index];
                    previewCell.SetColor(block.ColorIndex); // 강조 색상 인덱스
                    previewCell.transform.localPosition = boardPos;
                    previewCell.gameObject.SetActive(true);
                    index++;
                }
            }

            // 세로 줄 강조
            foreach (int x in matches.Columns)
            {
                for (int y = 0; y < height; y++)
                {
                    // 이미 가로에서 처리한 좌표는 중복 제거
                    if (matches.Rows.Contains(y)) { continue; }

                    if (_blockCellPreviewList.Count <= index)
                    {
                        var cell = Instantiate(_blockCellPreviewPrefab, this.transform).GetComponent<BlockCellView>();
                        _blockCellPreviewList.Add(cell);
                    }

                    Vector2Int pos = new Vector2Int(x, y);
                    Vector3 boardPos = _blockBoardView.CellToBoard(pos);
                    var previewCell = _blockCellPreviewList[index];
                    previewCell.SetColor(block.ColorIndex); // 강조 색상 인덱스
                    previewCell.transform.localPosition = boardPos;
                    previewCell.gameObject.SetActive(true);
                    index++;
                }
            }

            // 남은 미사용 프리뷰는 비활성화
            for (; index < _blockCellPreviewList.Count; index++)
            {
                _blockCellPreviewList[index].gameObject.SetActive(false);
            }
        }


    }
}