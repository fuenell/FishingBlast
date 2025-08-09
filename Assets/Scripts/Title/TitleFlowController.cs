using AppScope.Core;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Scene.Title
{
    public class TitleFlowController : IDisposable
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