using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Scene.Play
{
    public class BlockDragController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private BlockPlacementPreview _placementPreview;

        private Vector3 _blockToutchOffset = new Vector3(0, 3, 0);
        private BlockBoard _blockBoard;
        private BlockBoardView _blockBoardView;
        private BlockView _draggingBlock;

        [Inject]
        public void Construct(BlockBoard blockBoard, BlockBoardView blockBoardView, BlockPlacementPreview placementPreview)
        {
            _blockBoard = blockBoard;
            _blockBoardView = blockBoardView;
            _placementPreview = placementPreview;
        }

        public async UniTask<BlockModel> DragBlock()
        {
            _draggingBlock = null;

            Vector3 offset = Vector3.zero;

            // 1. 블럭 선택 기다리기
            while (_draggingBlock == null)
            {
                await UniTask.Yield();

                if (IsPress(out Vector2 screenPosition))
                {
                    Ray ray = _camera.ScreenPointToRay(screenPosition);
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("BlockView"));
                    if (hit.collider != null)
                    {
                        BlockView blockView = hit.collider.GetComponent<BlockView>();
                        if (blockView != null)
                        {
                            _draggingBlock = blockView;
                            _draggingBlock.StartDrag();
                            offset = _blockToutchOffset;
                        }
                    }
                }
            }

            // 2. 드래그 중 마우스 따라가기
            while (IsPress(out Vector2 pressScreenPosition))
            {
                Ray ray = _camera.ScreenPointToRay(pressScreenPosition);
                Vector3 worldPos = ray.origin + offset;
                _draggingBlock.transform.position = worldPos;

                _placementPreview.ShowPreview(_draggingBlock, worldPos); // 선택적으로 미리보기 출력

                await UniTask.Yield();
            }

            // 3. 마우스 놓았을 때 유효한 위치인지 판단
            Vector2Int cellPos = _blockBoardView.WorldToCell(_draggingBlock.Center);
            bool canPlace = _blockBoard.CanPlaceBlock(_draggingBlock.Model, cellPos);

            if (canPlace)
            {
                _blockBoard.PlaceBlock(_draggingBlock.Model, cellPos);
                _blockBoardView.PlaceBlock(_draggingBlock, cellPos);
                _placementPreview.Hide();

                var model = _draggingBlock.Model;

                Destroy(_draggingBlock.gameObject); // 여기서 파괴
                _draggingBlock = null;

                return model; // 모델만 반환
            }
            else
            {
                _draggingBlock.ResetPosition(); // 원위치 복귀
                _placementPreview.Hide();
                _draggingBlock = null;
                return null;
            }
        }

        private bool IsPress(out Vector2 screenPosition)
        {
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                return true;
            }
            else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                screenPosition = Mouse.current.position.ReadValue();
                return true;
            }

            screenPosition = Vector2.zero;
            return false;
        }
    }

}