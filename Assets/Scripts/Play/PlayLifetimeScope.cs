using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FishingBlast.Play
{
    public class PlayLifetimeScope : LifetimeScope
    {
        [SerializeField] private BlockQueuePresenter _blockQueuePresenter;
        [SerializeField] private BlockDragController _blockDragController;
        [SerializeField] private BlockBoardView _blockBoardView;
        [SerializeField] private BlockPlacementPreview _placementPreview;
        [SerializeField] private ScorePresenter _scorePresenter;
        [SerializeField] private CameraShaker _cameraShaker;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_blockQueuePresenter);
            builder.RegisterComponent(_blockDragController);
            builder.RegisterComponent(_blockBoardView);
            builder.RegisterComponent(_placementPreview);
            builder.RegisterComponent(_scorePresenter);
            builder.RegisterComponent(_cameraShaker);
            builder.RegisterComponentInHierarchy<PopupOpener>();

            // 예시: 씬에 있는 모든 MyComponent 타입의 오브젝트를 등록
            foreach (var myComponent in FindObjectsByType<BasePopup>(FindObjectsSortMode.None))
            {
                builder.RegisterInstance(myComponent);
            }

            builder.Register<BlockGenerator>(Lifetime.Scoped);
            builder.Register<BlockBoard>(Lifetime.Scoped);
            builder.Register<ScoreManager>(Lifetime.Scoped);

            builder.Register<PlayPopupManager>(Lifetime.Scoped).AsSelf().As<ITickable>();

            builder.RegisterEntryPoint<PlayFlowController>(Lifetime.Scoped);
        }
    }
}
