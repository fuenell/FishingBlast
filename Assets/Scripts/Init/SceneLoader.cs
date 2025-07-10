using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

public class SceneLoader
{
    private readonly AppLifetimeScope appScope;

    public SceneLoader(AppLifetimeScope appScope)
    {
        this.appScope = appScope;
    }

    public async UniTask Load(string scene)
    {
        using (LifetimeScope.EnqueueParent(appScope))
        {
            await SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        }
    }
}