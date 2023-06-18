using RoomByRoom.Control;
using UnityEngine;

namespace RoomByRoom.Gameplay.Initialization
{
  public class Bootstrap : MonoBehaviour
  {
    [SerializeField] private GameSaveSO _defaultGameSave; 
    private Configuration _config;
    private Engine _engine;
    private GameSaveService _gameSaveSvc;
    private OuterData _outerData;

    private void Awake()
    {
      _outerData = FindObjectOfType<OuterData>();
      _config = _outerData.Config;
      _gameSaveSvc = new GameSaveService(_outerData.ProfileName, _defaultGameSave.Value);
      
      _engine = FindObjectOfType<Engine>();
      _engine.Construct(_config, _gameSaveSvc);
    }
  }
}