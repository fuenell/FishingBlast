using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Scene.Play
{
    public class BlockDragController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Vector3 _blockToutchOffset = new Vector3(0, 3, 0);

        private BlockPlacementPreview _placementPreview;
        private BlockBoard _blockBoard;
        private BlockBoardView _blockBoardView;
        private PlayPopupManager _playPopupManager;

        private BlockView _draggingBlock;

        [Inject]
        public void Construct(BlockBoard blockBoard, BlockBoardView blockBoardView, BlockPlacementPreview placementPreview, PlayPopupManager playPopupManager)
        {
            _blockBoard = blockBoard;
            _blockBoardView = blockBoardView;
            _placementPreview = placementPreview;
            _playPopupManager = playPopupManager;
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

            Vector2Int lastGridPosition = Vector2Int.zero;
            bool lastCanPlace = false;

            // 2. 드래그 중 마우스 따라가기
            while (IsPress(out Vector2 pressScreenPosition))
            {
                Ray ray = _camera.ScreenPointToRay(pressScreenPosition);
                Vector3 worldPos = ray.origin + offset;
                _draggingBlock.transform.position = worldPos;

                // 3. 마우스 놓았을 때 유효한 위치인지 판단
                Vector2Int gridPosition = _blockBoardView.WorldToGrid(_draggingBlock.Center);
                bool canPlace = _blockBoard.CanPlaceBlock(_draggingBlock.Model, gridPosition);

                if (canPlace)
                {
                    var matches = _blockBoard.GetMatchedLinesIfPlaced(_draggingBlock.Model, gridPosition);
                    _placementPreview.ShowPreview(_draggingBlock, gridPosition, matches);

                    lastGridPosition = gridPosition;
                    lastCanPlace = canPlace;
                }
                else
                {
                    // 만약 이전 프레임이 canPlace 이고 최대 거리가 1칸 이내면 유지
                    if (lastCanPlace)
                    {
                        Vector2Int distanceVector = lastGridPosition - gridPosition;
                        int maxDistance = Mathf.Max(Mathf.Abs(distanceVector.x), Mathf.Abs(distanceVector.y));
                        // 유지 실패
                        if (1 < maxDistance)
                        {
                            lastCanPlace = canPlace;
                        }
                    }

                    if (lastCanPlace == false)
                    {
                        _placementPreview.HidePreview(); // 유효하지 않으면 미리보기 숨김
                    }
                }

                await UniTask.Yield();
            }

            // 3. 마우스 놓았을 때 유효한 위치인지 판단
            if (lastCanPlace)
            {
                _blockBoard.PlaceBlock(_draggingBlock.Model, lastGridPosition);
                _blockBoardView.PlaceBlock(_draggingBlock, lastGridPosition);
                _placementPreview.HidePreview();

                var model = _draggingBlock.Model;

                Destroy(_draggingBlock.gameObject); // 여기서 파괴
                _draggingBlock = null;

                return model; // 모델만 반환
            }
            else
            {
                _draggingBlock.ResetPosition(); // 원위치 복귀
                _placementPreview.HidePreview();
                _draggingBlock = null;
                return null;
            }
        }

        // Todo: 추후 인풋 매니저로 연결
        private bool IsPress(out Vector2 screenPosition)
        {
            // Todo: 추후 게임 흐름 매니저에게 요청
            if (_playPopupManager.IsAnyPopupOpen)
            {
                screenPosition = Vector2.zero;
                return false;
            }

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