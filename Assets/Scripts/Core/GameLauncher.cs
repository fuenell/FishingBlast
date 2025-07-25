using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace AppScope.Core
{
    public class GameLauncher : IStartable
    {
        private readonly GameInitializer _gameInitializer;
        private readonly SceneLoader _sceneLoader;

        public GameLauncher(GameInitializer gameInitializer, SceneLoader sceneLoader)
        {
            _gameInitializer = gameInitializer;
            _sceneLoader = sceneLoader;
        }

        public async void Start()
        {
            if (_sceneLoader.IsInitScene())
            {
                await UniTask.WaitUntil(() => _gameInitializer.IsInitialized);
                await _sceneLoader.LoadSceneAsync("Title");
            }
        }
    }
}