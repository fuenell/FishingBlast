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
            builder.RegisterInstance(blockPresenter); // 씬 오브젝트
            builder.Register<BlockGenerator>(Lifetime.Singleton);
            builder.Register<BlockBoard>(Lifetime.Singleton);
            builder.RegisterEntryPoint<PlayFlowController>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
