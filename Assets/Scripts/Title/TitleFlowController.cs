using AppScope.Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Scene.Title
{
    public class TitleFlowController
    {
        private SceneLoader _sceneLoader;

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