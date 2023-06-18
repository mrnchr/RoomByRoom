#if UNITY_EDITOR
using Leopotam.EcsLite.UnityEditor;
#endif
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using RoomByRoom.Config.Data;
using RoomByRoom.Control;
using RoomByRoom.Debugging;
using RoomByRoom.Scene;
using RoomByRoom.UI.Game;
using RoomByRoom.UI.Game.Inventory;
using RoomByRoom.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom
{
  public sealed class Engine : MonoBehaviour
  {
    // TODO: change to external injection
    [FormerlySerializedAs("_gameData"),FormerlySerializedAs("_initializeData"),SerializeField] private GameSaveSO _gameSaveSo;
    [SerializeField] private SceneInfo _sceneInfo;
    [SerializeField] private PrefabData _prefabData;
    [SerializeField] private SpriteData _spriteData;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private InventoryUpdater _inventoryUpdater;
    [SerializeField] private GameMediator _mediator;
    [SerializeField] private ItemDescriptionUpdater _itemDescUpdater;
    [SerializeField] private CharacteristicStrings _charStrings;
    [SerializeField] private MouseTracking _mouseTracking;
    private GameInfo _gameInfo;
    private EcsWorld _message;
    private PrefabService _prefabSvc;
    private GameSave _gameSave;
    private Configuration _config;

    private IEcsSystems _updateSystems;

    // private IEcsSystems _fixedUpdateSystems;
    private EcsWorld _world;
    private EquipService _equipSvc;
    private GameSaveService _gameSaveSvc;

    public void Construct(Configuration config, GameSaveService gameSaveSvc)
    {
      _config = config;
      _gameSaveSvc = gameSaveSvc;
    }
    
    private void Awake()
    {
      _gameSave = new GameSave();
      _prefabSvc = new PrefabService(_prefabData);
      _gameInfo = new GameInfo();

      _world = new EcsWorld();
      _message = new EcsWorld();
      _updateSystems = new EcsSystems(_world);

      _inventoryUpdater.Construct(_world, _message, new SpriteService(_spriteData));
      _itemDescUpdater.Construct(new CharacteristicConverter(_charStrings));
      _mouseTracking.Construct(_message);

#if UNITY_EDITOR
      _sceneInfo.CurrentGame = _gameInfo;
      _sceneInfo.CurrentSave = _gameSave;
      _sceneInfo.Config = _config;
#endif
    }

    private void Start()
    {
      var attackSvc = new AttackService(_world, _message);
      var charSvc = new CharacteristicService(_world);
      var blockingSvc = new BlockingService(_gameInfo);
      var keepDirtySvc = new KeepDirtyService(_message);
      var sceneSvc = new ScenePreloader();
      _equipSvc = new EquipService(_world, charSvc, keepDirtySvc);
      _mediator.Construct(new TurnWindowService(_message), _equipSvc, sceneSvc);
      _gameSave.Copy(_gameSaveSvc.LoadProfile());

      _updateSystems
        .AddWorld(_message, Idents.Worlds.MessageWorld)
        // IEcsInitSystems
        .Add(new LoadRoomSystem())
        .Add(new LoadPlayerSystem())
        .Add(new LoadInventorySystem())
        .Add(new LoadPlayerHandSystem())
        .Add(new PickPlayerMainWeaponSystem())
        .Add(new SetupGameSystem())
        // IEcsRunSystems
        .DelHere<GameLoadedMessage>(Idents.Worlds.MessageWorld)
        .Add(new LoadGameSystem())
        .Add(new WinUISystem())
        .Add(new PreparePlayerSystem())
        .Add(new CreateEnemySystem())
        .Add(new WearHumanoidEnemySystem())
        .Add(new FillEnemyEquipmentSystem())
        .DelHere<Bare>()
        .DelHere<AddPlayerCommand>()
        .Add(new CreateRoomViewSystem())
        .Add(new CreateUnitViewSystem())
        .Add(new CreateEquipmentViewSystem())
        .Add(new SpawnBonusSystem())
        .Add(new CreateSpawnPointSystem())
        .Add(new PutUnitInRoomSystem())
        .DelHere<OpenDoorMessage>(Idents.Worlds.MessageWorld)
        .DelHere<RotateCameraMessage>(Idents.Worlds.MessageWorld)
        .DelHere<GetDeveloperMessage>(Idents.Worlds.MessageWorld)
        .DelHere<MoveCommand>()
        .DelHere<RotateCommand>()
        .DelHere<JumpCommand>()
        .DelHere<AttackCommand>()
        .DelHere<TakeCommand>()
        .Add(new InputSystem())
        .Add(new GetDeveloperSystem())
        .Add(new EnemyAISystem())
        .Add(new TimerSystem<CantAttack>())
        .Add(new DelTimerSystem<CantAttack>())
        .Add(new DelayAttackSystem())
        .Add(new MoveSystem())
        .Add(new RotateUnitSystem())
        .Add(new TimerSystem<CantJump>())
        .Add(new DelTimerSystem<CantJump>())
        .Add(new JumpSystem())
        .Add(new AnimateLandingSystem())
        .Add(new RotateCameraSystem())
        .Add(new KeepCameraSystem())
        .Add(new AttackSystem())
        .Add(new TakeBonusSystem())
        .DelHere<WindowChangedMessage>(Idents.Worlds.MessageWorld)
        .Add(new WindowStateSystem())
        .Add(new BlockGameSystem())
        .Add(new DropItemSystem())
        .Add(new BreakDragSystem())
        .Add(new UpdateInventorySystem())
        .DelHere<StartGameMessage>(Idents.Worlds.MessageWorld)
        .DelHere<NextRoomMessage>(Idents.Worlds.MessageWorld)
        .Add(new OpenDoorSystem())
        .Add(new CheckWinSystem())
        .Add(new MarkCanDeletedSystem())
        .Add(new RemoveEntitySystem())
        .Add(new RecreateRoomSystem())
        .Add(new TimerSystem<CantRestore>())
        .Add(new DelTimerSystem<CantRestore>())
        .Add(new DamageSystem())
        .Add(new RestoreProtectionSystem())
        .DelHere<PlayerDyingMessage>(Idents.Worlds.MessageWorld)
        .Add(new CheckHealthSystem())
        .Add(new CreateBonusSystem())
        .Add(new DieSystem())
        .DelHere<RoomCleanedMessage>(Idents.Worlds.MessageWorld)
        .Add(new OpenNextRoomSystem())
        .DelHere<SelectCommand>()
        .DelHere<DeselectCommand>()
        .Add(new SelectBonusSystem())
        .Add(new HighlightBonusSystem())
        .Add(new ShowBonusCardSystem())
        .Add(new UpdateBarSystem())
        .Add(new UpdateDirtySystem())
        .Add(new UpdateItemInfoSystem())
        .Add(new ReloadGameSystem())
        .Add(new SaveGameSystem())
#if UNITY_EDITOR
        .Add(new AddInventoryInfoSystem())
        .Add(new CheckInventorySystem())
        .Add(new EcsWorldDebugSystem())
        .Add(new EcsWorldDebugSystem(Idents.Worlds.MessageWorld))
#endif
        .Inject(_sceneInfo, _gameSave, _prefabSvc, _config,
                _gameSaveSvc, _gameSaveSo, _gameInfo, attackSvc, _enemyData, charSvc, _playerData, blockingSvc,
                _mediator, keepDirtySvc, sceneSvc, _equipSvc)
        .Init();

      // _fixedUpdateSystems = new EcsSystems(_world);
      // _fixedUpdateSystems
      //     .Add(new RotateCameraSystem())
      //     .AddWorld(message, Idents.Worlds.MessageWorld)
      //     .Inject(_sceneData, _savedData, _packedPrefabData, _configuration)
      //     .Init();
    }

    private void Update()
    {
      _updateSystems?.Run();
    }

    // private void FixedUpdate()
    // {
    //     _fixedUpdateSystems?.Run();
    // }

    private void OnDestroy()
    {
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

      if (_message != null)
      {
        _message.Destroy();
        _message = null;
      }

      if (_world != null)
      {
        _world.Destroy();
        _world = null;
      }
    }
  }
}