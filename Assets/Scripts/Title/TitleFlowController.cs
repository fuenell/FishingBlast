
using UnityEngine;
using VContainer.Unity;

internal class TitleFlowController : IStartable
{
    private SceneLoader _sceneLoader;

    public TitleFlowController(SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;

        Debug.Log(this.GetType().Name);
    }

    public void Start()
    {
        Debug.Log(this.GetType().Name + "s");

        _sceneLoader.Load("");
    }
}