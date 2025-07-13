namespace Scene.Play
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class BlockView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private BlockModel _model;
        public System.Action OnPlaced;

        public void SetModel(BlockModel model)
        {
            _model = model;
            // 비주얼 구성
        }

        public void OnBeginDrag(PointerEventData eventData) { }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // 위치 체크 후 유효한 경우 판에 배치
            OnPlaced?.Invoke();
            Destroy(gameObject);
        }
    }

}