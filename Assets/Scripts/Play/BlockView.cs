namespace Scene.Play
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class BlockView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private Sprite _blockCell;

        [SerializeField]
        private float _blockCellCpacing = 1.2f;

        private BlockModel _model;
        public System.Action OnPlaced;

        public void SetModel(BlockModel model)
        {
            _model = model;
            // 비주얼 구성

            Vector4 rect = Vector4.zero;  // x.Min y.Min x.Max y.Max

            Transform cells = new GameObject("Cells").transform;
            cells.SetParent(this.transform, false);

            foreach (Vector2Int pos in _model.GetShape())
            {
                GameObject blockCell = new GameObject("Cell");
                blockCell.transform.parent = cells;
                blockCell.AddComponent<SpriteRenderer>().sprite = _blockCell;

                Vector2 cellPosition = new Vector2(pos.x, pos.y) * _blockCellCpacing;
                blockCell.transform.localPosition = cellPosition;

                rect.x = Mathf.Min(rect.x, pos.x);
                rect.y = Mathf.Min(rect.y, pos.y);
                rect.z = Mathf.Max(rect.z, pos.x);
                rect.w = Mathf.Max(rect.w, pos.y);
            }

            Vector2 centerOffset = new Vector2(rect.x + rect.z, rect.y + rect.w) * -0.5f * _blockCellCpacing;
            cells.localPosition = centerOffset;

            this.transform.localScale = Vector2.one * 0.5f;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            this.transform.localScale = Vector3.one;

            Vector3 screenPos = eventData.position;
            screenPos.z = Camera.main.WorldToScreenPoint(transform.position).z; // Z값 유지
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.y += 3;
            transform.position = worldPos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 screenPos = eventData.position;
            screenPos.z = Camera.main.WorldToScreenPoint(transform.position).z; // Z값 유지
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.y += 3;
            transform.position = worldPos;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // 위치 체크 후 유효한 경우 판에 배치
            OnPlaced?.Invoke();
            Destroy(gameObject);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
        }
        public void OnEndDrag(PointerEventData eventData)
        {
        }
    }

}