using RoomByRoom.Control;
using UnityEngine;

namespace RoomByRoom.Gameplay.Initialization
{
  public class Bootstrap : MonoBehaviour
  {
    [SerializeField] private ConfigurationSO _defaultConfig;
    private Configuration _config;

    private Engine _engine;
    private void Awake()
    {
      LoadConfig(new ConfigSavingService());
      _engine = FindObjectOfType<Engine>();
    }

    private void LoadConfig(ConfigSavingService cfgSavingSvc)
    {
      if (cfgSavingSvc.LoadData(ref _config)) return;
      _config = _defaultConfig.Value;
      cfgSavingSvc.SaveData(_config);
    }

    private void Start()
    {
      _engine.Construct(_config);
    }
  }
}