namespace FishingBlast.Play
{
    using FishingBlast.Data;
    using UnityEngine;

    public class BlockView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _blockCellPrefab;

        [SerializeField]
        private float _blockCellCpacing = 1.2f;

        private BlockModel _model;
        public BlockModel Model => _model;

        private Transform _cells;
        public Vector3 Center => _cells != null ? _cells.position : Vector3.zero;

        public int ColorIndex => _model.ColorIndex;

        public void SetModel(BlockModel model)
        {
            _model = model;
            // 비주얼 구성

            Vector4 rect = Vector4.zero;  // x.Min y.Min x.Max y.Max

            _cells = new GameObject("Cells").transform;
            _cells.SetParent(this.transform, false);

            foreach (Vector2Int pos in _model.Shape)
            {
                GameObject blockCell = Instantiate(_blockCellPrefab, _cells);
                blockCell.GetComponent<BlockCellView>().SetColor(ColorIndex);

                Vector2 cellPosition = new Vector2(pos.x, pos.y) * _blockCellCpacing;
                blockCell.transform.localPosition = cellPosition;
                blockCell.transform.localScale = Vector3.one;

                rect.x = Mathf.Min(rect.x, pos.x);
                rect.y = Mathf.Min(rect.y, pos.y);
                rect.z = Mathf.Max(rect.z, pos.x);
                rect.w = Mathf.Max(rect.w, pos.y);
            }

            Vector2 centerOffset = new Vector2(rect.x + rect.z, rect.y + rect.w) * -0.5f * _blockCellCpacing;
            _cells.localPosition = centerOffset;

            this.transform.localScale = Vector2.one * 0.5f;
        }

        public void ResetPosition()
        {
            // 위치 초기화
            this.transform.localPosition = Vector3.zero;
            this.transform.localScale = Vector3.one * 0.5f;

            // Todo: 그림자 보이기
        }

        // 드래그 시작
        public void StartDrag()
        {
            // 크기 정상
            this.transform.localScale = Vector3.one;

            // Todo: 그림자 숨기기
        }
    }

}