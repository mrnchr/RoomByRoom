using Infrastructure.SceneLoading;
using RoomByRoom.Infrastructure.Initializing;

namespace RoomByRoom.UI.MainMenu.ContinueButton
{
  public class ContinueGameButtonController : IContinueGameButtonController
  {
    private readonly IProgressLoader _loader;
    private readonly ISceneLoadingService _sceneLoading;

    public ContinueGameButtonController(IProgressLoader loader, ISceneLoadingService sceneLoading)
    {
      _loader = loader;
      _sceneLoading = sceneLoading;
    }

    public void Continue()
    {
      if(_loader.IsLoaded)
        _sceneLoading.LoadGameScene();
    }
  }
}