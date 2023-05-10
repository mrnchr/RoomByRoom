using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
  public class MainMenuMediator : MonoBehaviour
  {
    private MenuWindowSwitcher _windowSwitcher;
    private ProfileSelectorWindow _profileSelectorWindow;
    private ProfileGroupFactory _profileGroupFactory;
    private ProfileService _profileSvc;
    private StartGameService _startGameSvc;
    private ProfileCreatorWindow _profileCreatorWindow;

    private void Awake()
    {
      _windowSwitcher = FindObjectOfType<MenuWindowSwitcher>();
      _profileSelectorWindow = FindObjectOfType<ProfileSelectorWindow>();
      _profileCreatorWindow = FindObjectOfType<ProfileCreatorWindow>();
    }

    public void Construct(
      ProfileGroupFactory profileGroupFactory,
      ProfileService profileSvc,
      StartGameService startGameSvc)
    {
      _profileGroupFactory = profileGroupFactory;
      _profileSvc = profileSvc;
      _startGameSvc = startGameSvc;
    }

    public void SwitchProfile(bool isProfileActive)    => _windowSwitcher.SwitchProfile(isProfileActive);
    public void SwitchNewProfile(bool isProfileActive) => _windowSwitcher.SwitchNewProfile(isProfileActive);
    public string[] LoadProfiles()                     => _profileSvc.Load();
    public void DeleteProfile(string profileName)      => _profileSvc.Delete(profileName);
    public ProfileGroup CreateProfileGroup()           => _profileGroupFactory.Create();
    public void ShowProfiles()                         => _profileSelectorWindow.Show();
    public void ShowNewProfile()                       => _profileCreatorWindow.Show();
    public void TryStartGame()                         => _profileCreatorWindow.StartGame();
    public void StartGame(string profileName)          => _startGameSvc.StartGame(profileName);
    public void Exit()                                 => Application.Quit();
  }
}