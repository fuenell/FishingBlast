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

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_blockQueuePresenter); // 씬 오브젝트
            builder.RegisterComponent(_blockDragController); // 씬 오브젝트
            builder.RegisterComponent(_blockBoardView); // 씬 오브젝트
            builder.RegisterComponent(_placementPreview); // 씬 오브젝트

            builder.Register<BlockGenerator>(Lifetime.Scoped);
            builder.Register<BlockBoard>(Lifetime.Scoped);

            builder.RegisterEntryPoint<PlayFlowController>(Lifetime.Scoped);
        }
    }
}
