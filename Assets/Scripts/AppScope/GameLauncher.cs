using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace FishingBlast.AppScope
{
    public class GameLauncher : IStartable
    {
        private readonly GameInitializer _gameInitializer;
        private readonly SceneLoader _sceneLoader;
        private readonly DataManager _dataManager;

        public GameLauncher(GameInitializer gameInitializer, SceneLoader sceneLoader, DataManager dataManager)
        {
            _gameInitializer = gameInitializer;
            _sceneLoader = sceneLoader;
            _dataManager = dataManager;
        }

        public async void Start()
        {
            if (_sceneLoader.IsInitScene())
            {
                await UniTask.WaitUntil(() => _gameInitializer.IsInitialized);

                if (_dataManager.ShouldLoadPlayScene())
                {
                    await _sceneLoader.LoadSceneAsync("Play");
                }
                else
                {
                    await _sceneLoader.LoadSceneAsync("Title");
                }
            }
        }
    }
}