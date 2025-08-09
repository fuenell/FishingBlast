using VContainer;
using VContainer.Unity;

namespace FishingBlast.Title
{
    public class TitleLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<TitleFlowController>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
        }
    }
}