using FishingBlast.AppScope;
using Cysharp.Threading.Tasks;

namespace FishingBlast.Title
{
    public class TitleFlowController
    {
        private readonly SceneLoader _sceneLoader;

        private bool _isNavigating = false;

        public TitleFlowController(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public async UniTaskVoid OnClickStartButton()
        {
            if (_isNavigating)
            {
                return;
            }
            _isNavigating = true;

            await _sceneLoader.LoadSceneAsync("Play");
        }
    }
}