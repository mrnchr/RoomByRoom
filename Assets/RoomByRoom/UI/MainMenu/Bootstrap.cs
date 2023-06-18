using RoomByRoom.Config.Data;
using RoomByRoom.Control;
using RoomByRoom.Utility;
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

    private OuterData _outerData;
    private StartGameService _startGameSvc;
    private ProfileService _profileSvc;
    private ProfileGroupFactory _profileGroupFactory;

    private void Awake()
    {
      _outerData = new GameObject("OuterData").AddComponent<OuterData>()
        .SetConfig(new ConfigSaveService(Idents.FilePaths.ConfigFileName).Load());
      _outerData.transform.SetParent(null);
      DontDestroyOnLoad(_outerData);
      
      _profileSvc = new ProfileService();
      _profileGroupFactory = new ProfileGroupFactory(_profileGroup, _profileParent, _mainMenuMediator);
      _startGameSvc = new StartGameService(_outerData);

      _mainMenuMediator.Construct(_profileGroupFactory, _profileSvc, _startGameSvc);
    }
  }
}