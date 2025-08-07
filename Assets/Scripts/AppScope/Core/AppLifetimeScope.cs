using VContainer;
using VContainer.Unity;

namespace AppScope.Core
{
    public class AppLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SoundManager>(Lifetime.Singleton);
            builder.Register<SceneLoader>(Lifetime.Singleton);
            builder.Register<AdsManager>(Lifetime.Singleton);
            builder.Register<InputService>(Lifetime.Singleton);
            builder.Register<DataManager>(Lifetime.Singleton);

            builder.Register<GameInitializer>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GameLauncher>(); // 씬에 존재하는 컴포넌트 등록
        }

        //builder.Register<GameLauncher>(Lifetime.Singleton).AsImplementedInterfaces();
        //builder.Register<SaveDataService>(Lifetime.Singleton);
    }
}