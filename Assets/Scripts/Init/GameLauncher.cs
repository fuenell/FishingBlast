using VContainer.Unity;

public class GameLauncher : IStartable
{
    private SceneLoader _sceneLoader;

    public GameLauncher(SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    public void Start()
    {
        _sceneLoader.Load("Title");
    }
}
