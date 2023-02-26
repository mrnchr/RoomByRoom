using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;

using RoomByRoom.Utility;
using RoomByRoom.Debug;

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
                .AddWorld(new EcsWorld(), Idents.Worlds.MessageWorld)

                .Add(new LoadRoomSystem())
                .Add(new LoadPlayerSystem())
                .DelHere<AddPlayerCommand>()
                .Add(new CreateRoomViewSystem())
                .Add(new CreatePlayerViewSystem())
                .DelHere<SpawnPoint>()
                .Add(new CreateSpawnPointSystem())
                .Add(new PutUnitInRoomSystem())
                .DelHere<OpenDoorMessage>(Idents.Worlds.MessageWorld)
                .DelHere<RotateCameraMessage>(Idents.Worlds.MessageWorld)
                .DelHere<MoveCommand>()
                .DelHere<JumpCommand>()
                .Add(new InputSystem())
                .Add(new MoveUnitSystem())
                .Add(new RotateUnitSystem())
                .Add(new JumpUnitSystem())
                .Add(new AfterJumpUnitSystem())
                .Add(new RotateCameraSystem())
                .DelHere<StartGameMessage>(Idents.Worlds.MessageWorld)
                .DelHere<NextRoomMessage>(Idents.Worlds.MessageWorld)
                .Add(new OpenDoorSystem())
                .Add(new RecreateRoomSystem())
                .Add(new CreateEnemySystem())
                .Add(new CreateEnemyViewSystem())

#if UNITY_EDITOR
                .Add(new MarkEnemySystem())
                .Add(new RemoveEnemySystem())
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(Idents.Worlds.MessageWorld))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(bakeComponentsInName: false))
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