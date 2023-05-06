using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
  public class ProfileButtonFactory : IFactory<ProfileView>
  {
    private readonly ProfileView _prefab;
    private readonly Transform _parent;
    private readonly MainMenuMediator _mainMenuMediator;

    public ProfileButtonFactory(ProfileView prefab, Transform parent, MainMenuMediator mainMenuMediator)
    {
      _prefab = prefab;
      _parent = parent;
      _mainMenuMediator = mainMenuMediator;
    }

    public ProfileView Create()
    {
      ProfileView view = Object.Instantiate(_prefab, _parent);
      view.OnSelect += _mainMenuMediator.StartGame;

      return view;
    }
  }
}