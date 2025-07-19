using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Scene.Play
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

        public void ShowPreview(BlockView block, Vector2Int cellPos)
        {
            int index = 0;

            foreach (Vector2Int blockPos in block.Model.GetShape())
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

        public void Hide()
        {
            _blockCellPreviewList.ForEach(cell => cell.gameObject.SetActive(false));
        }
    }
}