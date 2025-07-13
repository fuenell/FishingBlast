using VContainer.Unity;
using VContainer;

namespace Scene.Title
{
    public class TitleLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<TitleFlowController>(Lifetime.Scoped);
        }
    }
}