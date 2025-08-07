using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace AppScope.Core
{
    public class SceneLoader
    {
        private readonly AppLifetimeScope appScope;

        public SceneLoader(AppLifetimeScope appScope)
        {
            this.appScope = appScope;
        }

        public async UniTask LoadSceneAsync(string scene)
        {
            using (LifetimeScope.EnqueueParent(appScope))
            {
                await SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
            }
        }

        public bool IsInitScene()
        {
            return SceneManager.GetActiveScene().name == "Init";
        }
    }
}