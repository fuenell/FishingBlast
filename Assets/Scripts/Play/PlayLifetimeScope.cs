using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scene.Play
{
    public class PlayLifetimeScope : LifetimeScope
    {
        [SerializeField] private BlockQueuePresenter _blockQueuePresenter;
        [SerializeField] private BlockDragController _blockDragController;
        [SerializeField] private BlockBoardView _blockBoardView;
        [SerializeField] private BlockPlacementPreview _placementPreview;
        [SerializeField] private ScorePresenter _scorePresenter;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_blockQueuePresenter);
            builder.RegisterComponent(_blockDragController);
            builder.RegisterComponent(_blockBoardView);
            builder.RegisterComponent(_placementPreview);
            builder.RegisterComponent(_scorePresenter);

            builder.Register<BlockGenerator>(Lifetime.Scoped);
            builder.Register<BlockBoard>(Lifetime.Scoped);
            builder.Register<ScoreManager>(Lifetime.Scoped);

            builder.RegisterEntryPoint<PlayFlowController>(Lifetime.Scoped);
        }
    }
}
