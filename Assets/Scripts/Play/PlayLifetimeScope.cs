using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scene.Play
{
    public class PlayLifetimeScope : LifetimeScope
    {
        [SerializeField] private BlockPresenter blockPresenter;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance<BlockPresenter>(blockPresenter); // 씬 오브젝트
            builder.Register<BlockGenerator>(Lifetime.Scoped);
            builder.Register<BlockBoard>(Lifetime.Scoped);
            builder.RegisterEntryPoint<PlayFlowController>(Lifetime.Scoped);
        }
    }
}
