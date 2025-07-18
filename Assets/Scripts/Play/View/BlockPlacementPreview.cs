using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Scene.Play
{
    public class BlockPlacementPreview : MonoBehaviour
    {
        [SerializeField] private GameObject _blockCellPreviewPrefab;

        private BlockBoardView _blockBoardView;

        private List<GameObject> _blockCellPreviewList = new List<GameObject>();

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
                    GameObject blockCellPreview = Instantiate(_blockCellPreviewPrefab, this.transform);
                    _blockCellPreviewList.Add(blockCellPreview);
                }

                Vector2Int placeCellPos = blockPos + cellPos;
                Vector3 boardPosition = _blockBoardView.CellToBoard(placeCellPos);
                _blockCellPreviewList[index].SetActive(true);
                _blockCellPreviewList[index].transform.localPosition = boardPosition;
                index++;
            }

            for (; index < _blockCellPreviewList.Count; index++)
            {
                _blockCellPreviewList[index].SetActive(false);
            }
        }

        public void Hide()
        {
            _blockCellPreviewList.ForEach(preview => preview.SetActive(false));
        }
    }
}