using Infrastructure.SceneLoading;
using RoomByRoom.Web.Utils;

namespace RoomByRoom.UI.MainMenu.LogoutButtonView
{
  public class LogoutButtonController : ILogoutButtonController
  {
    private readonly ISceneLoadingService _sceneLoading;
    private readonly ITokenProvider _tokenProvider;

    public LogoutButtonController(ISceneLoadingService sceneLoading, ITokenProvider tokenProvider)
    {
      _sceneLoading = sceneLoading;
      _tokenProvider = tokenProvider;
    }

    public void Logout()
    {
      _tokenProvider.Token = null;
      _sceneLoading.LoadSignScene();
    }
  }
}