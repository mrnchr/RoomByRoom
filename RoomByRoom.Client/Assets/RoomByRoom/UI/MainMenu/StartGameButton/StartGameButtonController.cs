using Infrastructure.SceneLoading;
using RoomByRoom.Infrastructure.Initializing;

namespace RoomByRoom.UI.MainMenu.StartButton
{
  public class StartGameButtonController : IStartGameButtonController
  {
    private readonly IProgressLoader _loader;
    private readonly IGameSaveService _saveSvc;
    private readonly ISceneLoadingService _sceneLoading;

    public StartGameButtonController(IProgressLoader loader, IGameSaveService saveSvc, ISceneLoadingService sceneLoading)
    {
      _loader = loader;
      _saveSvc = saveSvc;
      _sceneLoading = sceneLoading;
    }

    public void StartGame()
    {
      if (_loader.IsLoaded)
      {
        _saveSvc.SaveDefault();
        _sceneLoading.LoadGameScene();
      }
    }
  }
}