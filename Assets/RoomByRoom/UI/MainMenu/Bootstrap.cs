using RoomByRoom.Config.Data;
using RoomByRoom.Control;
using RoomByRoom.Initialization;
using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
  [DefaultExecutionOrder(-1)]
  public class Bootstrap : MonoBehaviour
  {
    [Header("Profile Creator Window Construction"), SerializeField]
		private ProfileCreatorWindow _profileCreatorWindow;

    [Header("Mediator Construction"), SerializeField]
		private MainMenuMediator _mainMenuMediator;

    [Header("Profile Button Factory Construction"), SerializeField]
		private ProfileGroup _profileGroup;
    [SerializeField] private Transform _profileParent;

    [Header("Others")]
    [SerializeField] private DefaultData _defaultData;
    [SerializeField] private ConfigurationSO _configSo;
    [SerializeField] private Configuration _config;
    [SerializeField] private OuterData _outerData;

    private StartGameService _startGameSvc;
    private ProfileService _profileSvc;
    private ProfileGroupFactory _profileGroupFactory;

    private void Awake()
    {
      Environment.Prepare(_configSo.Value.SaveInFile);

      _outerData.transform.SetParent(null);
      DontDestroyOnLoad(_outerData);
      LoadConfig(new ConfigSavingService());
      _profileSvc = new ProfileService(_config.SaveInFile);
      _profileGroupFactory = new ProfileGroupFactory(_profileGroup, _profileParent, _mainMenuMediator);
      _startGameSvc = new StartGameService(_outerData);

      _profileCreatorWindow.Construct(_defaultData.ProfileName);
      _mainMenuMediator.Construct(_profileGroupFactory, _profileSvc, _startGameSvc);
    }
    
    private void LoadConfig(ConfigSavingService cfgSavingSvc)
    {
      if (cfgSavingSvc.LoadData(ref _config)) return;
      _config = _configSo.Value;
      cfgSavingSvc.SaveData(_config);
    }
  }
}