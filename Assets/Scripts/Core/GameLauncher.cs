using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace AppScope.Core
{
    public class GameLauncher : IStartable
    {
        private GameInitializer _gameInitializer;
        private SceneLoader _sceneLoader;

        [Inject]
        public void Construct(GameInitializer gameInitializer, SceneLoader sceneLoader)
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