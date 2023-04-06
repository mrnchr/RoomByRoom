using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.UnityEditor;

using RoomByRoom.Debugging;
using RoomByRoom.Utility;

namespace RoomByRoom
{
    sealed class BootStrap : MonoBehaviour
    {
        // TODO: change to external injection
        [SerializeField] private DefaultData _defaultData;
        [SerializeField] private SceneData _sceneData;
        [SerializeField] private PrefabData _prefabData;
        [SerializeField] private Configuration _configuration;
        private SavedData _savedData;
        private GameInfo _gameInfo;
        private PackedPrefabData _packedPrefabData;
        private IEcsSystems _updateSystems;
        // private IEcsSystems _fixedUpdateSystems;
        private EcsWorld _world;

        private void Start () {
            _savedData = new SavedData();

            var savingSvc = new SavingService(_configuration.DefaultSaveName, _configuration.SaveInFile);
            _packedPrefabData = new PackedPrefabData(_prefabData);

            _gameInfo = new GameInfo();

            _sceneData.CurrentGame = _gameInfo;
            _sceneData.CurrentSave = _savedData;

            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world);
            var attackSvc = new AttackService(_world);

            _updateSystems
                .AddWorld(new EcsWorld(), Idents.Worlds.MessageWorld)

                .Add(new LoadSaveSystem())
                .Add(new LoadRoomSystem())
                .Add(new LoadPlayerSystem())
                .Add(new LoadInventorySystem())
                .Add(new PickPlayerMainWeaponSystem())
                .Add(new CreateEnemySystem())
                .Add(new WearHumanoidEnemySystem())

                .DelHere<AddPlayerCommand>()
                .Add(new CreateRoomViewSystem())
                .Add(new CreateUnitViewSystem())
                .Add(new CreateEquipmentViewSystem())

                .Add(new CreateSpawnPointSystem())
                .Add(new PutUnitInRoomSystem())

                .DelHere<OpenDoorMessage>(Idents.Worlds.MessageWorld)
                .DelHere<RotateCameraMessage>(Idents.Worlds.MessageWorld)
                .DelHere<MoveCommand>()
                .DelHere<RotateCommand>()
                .DelHere<JumpCommand>()
                .DelHere<AttackCommand>()
                .Add(new InputSystem())
                .Add(new EnemyAISystem())
                .Add(new MoveUnitSystem())
                .Add(new RotateUnitSystem())
                .Add(new JumpUnitSystem())
                .Add(new AfterJumpUnitSystem())
                .Add(new RotateCameraSystem())
                .Add(new AttackSystem())

                .DelHere<StartGameMessage>(Idents.Worlds.MessageWorld)
                .DelHere<NextRoomMessage>(Idents.Worlds.MessageWorld)
                .Add(new OpenDoorSystem())
                .Add(new RecreateRoomSystem())

                .Add(new DamageSystem())
                .Add(new DieSystem())
#if UNITY_EDITOR
                .Add(new MarkEnemySystem())
                .Add(new RemoveEnemySystem())
                .Add(new EcsWorldDebugSystem())
                .Add(new EcsWorldDebugSystem(Idents.Worlds.MessageWorld))
#endif
                .Inject(_sceneData, _savedData, _packedPrefabData, _configuration,
                savingSvc, _defaultData, _gameInfo, attackSvc)
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

        private void OnDestroy () {
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