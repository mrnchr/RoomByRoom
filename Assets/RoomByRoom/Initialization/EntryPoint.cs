using RoomByRoom.Control;
using RoomByRoom.Scene;
using UnityEngine;

namespace RoomByRoom.Initialization
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private ConfigurationSO _defaultCfg;
        private readonly ScenePreloader _sceneSvc = new ScenePreloader();

        private void Awake()
        {
            _sceneSvc.PreloadScene(1);
            Environment.LoadLocalFiles(_defaultCfg);
        }

        private void Start()
        {
            _sceneSvc.AllowSceneActivation();
        }
    }
}
