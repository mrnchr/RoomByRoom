using RoomByRoom.Initialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom.UI.MainMenu
{
  [DefaultExecutionOrder(-1)]
  public class Bootstrap : MonoBehaviour
  {
    [Header("Profile Creator Window Construction"), SerializeField]
		 private ProfileCreatorWindow _profileCreatorWindow;

    [SerializeField] private ProfileFieldView _profileFieldView;
    [SerializeField] private GameObject _errorMessageObject;

    [Header("Window Switcher Construction"), SerializeField]
		 private MenuWindowSwitcher _windowSwitcher;

    [SerializeField] private GameObject _buttonWindow;
    [SerializeField] private GameObject _profileWindow;
    [SerializeField] private GameObject _newProfileWindow;

    [Header("Mediator Construction"), SerializeField]
		 private MainMenuMediator _mainMenuMediator;

    [Header("Profile Button Factory Construction"), SerializeField]
		 private ProfileGroup _profileGroup;

    [SerializeField] private Transform _profileParent;

    [Header("Others"), SerializeField]
		 private Configuration _config;

    [SerializeField] private OuterData _outerData;

    private StartGameService _startGameSvc;
    private ProfileService _profileSvc;
    private ProfileGroupFactory _profileGroupFactory;

    private void Awake()
    {
      Environment.Prepare();

      _outerData.transform.SetParent(null);
      DontDestroyOnLoad(_outerData);
      _profileSvc = new ProfileService(_config.SaveInFile);
      _profileGroupFactory = new ProfileGroupFactory(_profileGroup, _profileParent, _mainMenuMediator);
      _startGameSvc = new StartGameService(_outerData);

      _profileCreatorWindow.Construct(_config, _profileFieldView, _errorMessageObject);
      _windowSwitcher.Construct(_buttonWindow, _profileWindow, _newProfileWindow);
      _mainMenuMediator.Construct(_profileGroupFactory, _profileSvc, _startGameSvc);
    }
  }
}