using RoomByRoom.UI.MainMenu.ContinueButton;
using RoomByRoom.UI.MainMenu.DeleteButton;
using RoomByRoom.UI.MainMenu.LogoutButtonView;
using RoomByRoom.UI.MainMenu.StartButton;
using Zenject;

namespace RoomByRoom.Infrastructure.Initializing
{
  public class MenuInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      BindProgressLoader();
      BindContinueGameButtonController();
      BindStartGameButtonController();
      BindLogoutButtonController();
      BindDeleteButtonController();
    }

    private void BindDeleteButtonController()
    {
      Container
        .Bind<IDeleteButtonController>()
        .To<DeleteButtonController>()
        .AsSingle();
    }

    private void BindLogoutButtonController()
    {
      Container
        .Bind<ILogoutButtonController>()
        .To<LogoutButtonController>()
        .AsSingle();
    }

    private void BindStartGameButtonController()
    {
      Container
        .Bind<IStartGameButtonController>()
        .To<StartGameButtonController>()
        .AsSingle();
    }

    private void BindContinueGameButtonController()
    {
      Container
        .Bind<IContinueGameButtonController>()
        .To<ContinueGameButtonController>()
        .AsSingle();
    }

    private void BindProgressLoader()
    {
      Container
        .BindInterfacesTo<ProgressLoader>()
        .AsSingle();
    }
  }
}