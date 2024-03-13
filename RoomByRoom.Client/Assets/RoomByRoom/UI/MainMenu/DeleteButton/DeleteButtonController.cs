using Infrastructure.SceneLoading;
using Profile;
using RoomByRoom.Web.Utils;

namespace RoomByRoom.UI.MainMenu.DeleteButton
{
  public class DeleteButtonController : IDeleteButtonController
  {
    private readonly ISceneLoadingService _sceneLoading;
    private readonly IProfileWebService _profileSvc;
    private readonly ITokenProvider _provider;

    public DeleteButtonController(ISceneLoadingService sceneLoading, IProfileWebService profileSvc, ITokenProvider provider)
    {
      _sceneLoading = sceneLoading;
      _profileSvc = profileSvc;
      _provider = provider;
    }

    public void Delete()
    {
      DeleteAsync();
    }

    private async void DeleteAsync()
    {
      await _profileSvc.DeleteUserAsync();
      _provider.Token = null;
      _sceneLoading.LoadSignScene();
    }
  }
}