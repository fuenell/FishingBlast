using AppScope.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Scene.Play
{
    public class BlockDragController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Vector3 _blockTouchOffset = new Vector3(0, 3, 0);

        private BlockPlacementPreview _placementPreview;
        private BlockBoard _blockBoard;
        private BlockBoardView _blockBoardView;
        private InputService _inputService;

        private BlockView _draggingBlock;

        [Inject]
        public void Construct(BlockBoard blockBoard, BlockBoardView blockBoardView, BlockPlacementPreview placementPreview, InputService inputService)
        {
            _blockBoard = blockBoard;
            _blockBoardView = blockBoardView;
            _placementPreview = placementPreview;
            _inputService = inputService;
        }

        public async UniTask<BlockModel> DragBlock()
        {
            _draggingBlock = await WaitForBlockSelection();
            _draggingBlock.StartDrag();

            (bool canPlace, Vector2Int gridPosition) = await TrackBlockMovement(_draggingBlock);

            if (canPlace)
            {
                return PlaceBlock(gridPosition);
            }
            else
            {
                CancelDrag();
                return null;
            }
        }

        private async UniTask<BlockView> WaitForBlockSelection()
        {
            while (true)
            {
                await UniTask.Yield();

                BlockView selectedBlock = TrySelectBlock();
                if (selectedBlock != null)
                {
                    return selectedBlock;
                }
            }
        }

        private async UniTask<(bool canPlace, Vector2Int gridPosition)> TrackBlockMovement(BlockView draggingBlock)
        {
            Vector2Int lastValidPosition = Vector2Int.zero;
            bool isLastPlaceable = false;

            while (_draggingBlock != null && _inputService.IsPressScreen(out Vector2 screenPosition))
            {
                UpdateDraggingBlockPosition(screenPosition);

                Vector2Int gridPosition = _blockBoardView.WorldToGrid(_draggingBlock.Center);
                bool canPlace = _blockBoard.CanPlaceBlock(_draggingBlock.Model, gridPosition);

                if (canPlace)
                {
                    isLastPlaceable = true;
                    lastValidPosition = gridPosition;

                    var matches = _blockBoard.GetMatchedLinesIfPlaced(_draggingBlock.Model, gridPosition);
                    _placementPreview.ShowPreview(_draggingBlock, gridPosition, matches);
                }
                else if (ShouldInvalidateLastPosition(gridPosition, lastValidPosition, isLastPlaceable))
                {
                    isLastPlaceable = false;
                    _placementPreview.HidePreview();
                }

                await UniTask.Yield();
            }

            return (isLastPlaceable, lastValidPosition);
        }

        private void UpdateDraggingBlockPosition(Vector2 screenPosition)
        {
            Ray ray = _camera.ScreenPointToRay(screenPosition);
            Vector3 worldPosition = ray.origin + _blockTouchOffset;
            _draggingBlock.transform.position = worldPosition;
        }

        private bool ShouldInvalidateLastPosition(Vector2Int current, Vector2Int lastValid, bool wasPlaceable)
        {
            if (!wasPlaceable)
            {
                return false;
            }

            Vector2Int delta = lastValid - current;
            int maxDistance = Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
            return maxDistance > 1;
        }

        private BlockModel PlaceBlock(Vector2Int position)
        {
            _blockBoard.PlaceBlock(_draggingBlock.Model, position);
            _blockBoardView.PlaceBlock(_draggingBlock, position);
            _placementPreview.HidePreview();

            BlockModel model = _draggingBlock.Model;
            Destroy(_draggingBlock.gameObject);
            _draggingBlock = null;

            return model;
        }

        private void CancelDrag()
        {
            _draggingBlock.ResetPosition();
            _placementPreview.HidePreview();
            _draggingBlock = null;
        }

        private BlockView TrySelectBlock()
        {
            if (!_inputService.IsPressScreen(out Vector2 screenPosition))
            {
                return null;
            }

            Ray ray = _camera.ScreenPointToRay(screenPosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("BlockView"));

            if (hit.collider != null && hit.collider.TryGetComponent(out BlockView blockView))
            {
                return blockView;
            }

            return null;
        }
    }
}
