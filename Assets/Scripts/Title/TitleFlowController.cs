using AppScope.Core;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Scene.Title
{
    public class TitleFlowController : IDisposable
    {
        private SceneLoader _sceneLoader;

        private bool _isNavigating = false;

        public TitleFlowController(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Dispose()
        {
            Debug.Log("TitleFlowController 삭제됨");
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