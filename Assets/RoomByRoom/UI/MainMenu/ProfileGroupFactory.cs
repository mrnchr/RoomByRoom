using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
  public class ProfileGroupFactory : IFactory<ProfileGroup>
  {
    private readonly ProfileGroup _prefab;
    private readonly Transform _parent;
    private readonly MainMenuMediator _mainMenuMediator;

    public ProfileGroupFactory(ProfileGroup prefab, Transform parent, MainMenuMediator mainMenuMediator)
    {
      _prefab = prefab;
      _parent = parent;
      _mainMenuMediator = mainMenuMediator;
    }

    public ProfileGroup Create()
    {
      ProfileGroup group = Object.Instantiate(_prefab, _parent);
      group.Profile.OnSelect += _mainMenuMediator.StartGame;
      group.OnSelect += _mainMenuMediator.DeleteProfile;
      group.Delete.OnSelect += group.DeleteProfile;

      return group;
    }
  }
}