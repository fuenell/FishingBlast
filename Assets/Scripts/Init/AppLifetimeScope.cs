using VContainer;
using VContainer.Unity;

public class AppLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<SoundManager>(Lifetime.Singleton);
        builder.Register<SceneLoader>(Lifetime.Singleton);
        //builder.Register<SaveDataService>(Lifetime.Singleton);

        //builder.Register<GameLauncher>(Lifetime.Singleton).AsImplementedInterfaces();
    }
}
