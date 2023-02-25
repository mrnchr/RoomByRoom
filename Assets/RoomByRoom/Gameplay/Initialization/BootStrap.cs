using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom 
{
    sealed class BootStrap : MonoBehaviour 
    {
        [SerializeField] private SceneData _sceneData;
        [SerializeField] private SavedData _savedData;
        [SerializeField] private PrefabData _prefabData;
        [SerializeField] private Configuration _configuration;
        private PackedPrefabData _packedPrefabData;
        private IEcsSystems _updateSystems;
        // private IEcsSystems _fixedUpdateSystems;
        private EcsWorld _world;        

        void Start () {
            _packedPrefabData = new PackedPrefabData(_prefabData);
            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world);
            _updateSystems
                .Add(new LoadRoomSystem())
                .Add(new CreateRoomViewSystem())
                .Add(new LoadPlayerSystem())
                .Add(new CreatePlayerViewSystem())
                .Add(new PutPlayerInRoomSystem())
                .Add(new InputSystem())
                .Add(new OpenDoorSystem())
                .Add(new RecreateRoomSystem())
                .Add(new MoveUnitSystem())
                .Add(new RotateUnitSystem())
                .Add(new JumpUnitSystem())
                .Add(new AfterJumpUnitSystem())
                .Add(new CreateSpawnPointSystem())
                .Add(new CreateEnemySystem())
                .Add(new RotateCameraSystem())
                .DelHere<NoPlayer>()
                .DelHere<MoveCommand>()
                .DelHere<JumpCommand>()

                .AddWorld(new EcsWorld(), Idents.Worlds.MessageWorld)
                .DelHere<NextRoomMessage>(Idents.Worlds.MessageWorld)
                .DelHere<OpenDoorMessage>(Idents.Worlds.MessageWorld)
                .DelHere<AddPlayerMessage>(Idents.Worlds.MessageWorld)
                .DelHere<StartGameMessage>(Idents.Worlds.MessageWorld)
                .DelHere<RotateCameraMessage>(Idents.Worlds.MessageWorld)
#if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem (Idents.Worlds.MessageWorld))
#endif
                .Inject(_sceneData, _savedData, _packedPrefabData, _configuration)
                .Init();

            // _fixedUpdateSystems = new EcsSystems(_world);
            // _fixedUpdateSystems
            //     .Add(new RotateCameraSystem())
            //     .AddWorld(message, Idents.Worlds.MessageWorld)
            //     .Inject(_sceneData, _savedData, _packedPrefabData, _configuration)
            //     .Init();
        }

        private void Update () 
        {
            _updateSystems?.Run();
        }

        private void FixedUpdate()
        {
            // _fixedUpdateSystems?.Run();
        }

        void OnDestroy () {
            if (_updateSystems != null) 
            {
                _updateSystems.Destroy();
                _updateSystems = null;
            }  

            // if (_fixedUpdateSystems != null) 
            // {
            //     _fixedUpdateSystems.Destroy();
            //     _fixedUpdateSystems = null;
            // }      
            
            if (_world != null) 
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}