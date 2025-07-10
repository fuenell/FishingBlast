using VContainer.Unity;
using VContainer;

public class TitleLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<TitleFlowController>();
        // 타이틀 관련 서비스
    }
}
