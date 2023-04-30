using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.UnityEditor;
using RoomByRoom.Debugging;
using RoomByRoom.UI.MainMenu;
using RoomByRoom.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom
{
	public sealed class Engine : MonoBehaviour
	{
		// TODO: change to external injection
		[SerializeField]
		private InitializeData _initializeData;

		[FormerlySerializedAs("_sceneData"), SerializeField]
		private SceneInfo _sceneInfo;

		[SerializeField] private PrefabData _prefabData;
		[SerializeField] private Configuration _configuration;
		[SerializeField] private EnemyData _enemyData;
		[SerializeField] private PlayerData _playerData;
		private GameInfo _gameInfo;
		private EcsWorld _message;
		private PackedPrefabData _packedPrefabData;
		private Saving _saving;

		private IEcsSystems _updateSystems;

		// private IEcsSystems _fixedUpdateSystems;
		private EcsWorld _world;

		private void Awake()
		{
			_saving = new Saving();
			_packedPrefabData = new PackedPrefabData(_prefabData);
			_gameInfo = new GameInfo();

			_world = new EcsWorld();
			_message = new EcsWorld();
			_updateSystems = new EcsSystems(_world);

#if UNITY_EDITOR
			_sceneInfo.CurrentGame = _gameInfo;
			_sceneInfo.CurrentSave = _saving;
#endif
		}

		private void Start()
		{
			var outerData = FindObjectOfType<OuterData>();
			var savingSvc = new SavingService(outerData.ProfileName, _configuration.SaveInFile);
			var attackSvc = new AttackService(_world, _message);
			var charSvc = new CharacteristicService(_world);

			_updateSystems
				.AddWorld(_message, Idents.Worlds.MessageWorld)
				.Add(new LoadSavingSystem())
				.Add(new EnterGameStateSystem())
				.Add(new LoadRoomSystem())
				.Add(new LoadPlayerSystem())
				.Add(new LoadInventorySystem())
				.Add(new PickPlayerMainWeaponSystem())
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
				.DelHere<MoveCommand>()
				.DelHere<RotateCommand>()
				.DelHere<JumpCommand>()
				.DelHere<AttackCommand>()
				.DelHere<TakeCommand>()
				.Add(new InputSystem())
				.Add(new EnemyAISystem())
				.Add(new TimerSystem<CantAttack>())
				.Add(new DelTimerSystem<CantAttack>())
				.Add(new DelayAttackSystem())
				.Add(new MoveUnitSystem())
				.Add(new RotateUnitSystem())
				.Add(new TimerSystem<CantJump>())
				.Add(new DelTimerSystem<CantJump>())
				.Add(new JumpUnitSystem())
				.Add(new AnimateLandingSystem())
				.Add(new RotateCameraSystem())
				.Add(new KeepCameraSystem())
				.Add(new AttackSystem())
				.Add(new TakeBonusSystem())
				.DelHere<StartGameMessage>(Idents.Worlds.MessageWorld)
				.DelHere<NextRoomMessage>(Idents.Worlds.MessageWorld)
				.Add(new OpenDoorSystem())
				.Add(new RecreateRoomSystem())
				.Add(new TimerSystem<CantRestore>())
				.Add(new DelTimerSystem<CantRestore>())
				.Add(new DamageSystem())
				.Add(new RestoreProtectionSystem())
				.DelHere<SpawnCommand>()
				.Add(new DieSystem())
				.DelHere<SelectCommand>()
				.DelHere<DeselectCommand>()
				.Add(new SelectBonusSystem())
				.Add(new HighlightBonusSystem())
#if UNITY_EDITOR
				.Add(new MarkEnemySystem())
				.Add(new RemoveEntitySystem())
				.Add(new AddInventoryInfoSystem())
				.Add(new CheckInventorySystem())
				.Add(new EcsWorldDebugSystem())
				.Add(new EcsWorldDebugSystem(Idents.Worlds.MessageWorld))
#endif
				.Inject(_sceneInfo, _saving, _packedPrefabData, _configuration,
					savingSvc, _initializeData, _gameInfo, attackSvc, _enemyData, charSvc, _playerData)
				.Init();

			// _fixedUpdateSystems = new EcsSystems(_world);
			// _fixedUpdateSystems
			//     .Add(new RotateCameraSystem())
			//     .AddWorld(message, Idents.Worlds.MessageWorld)
			//     .Inject(_sceneData, _savedData, _packedPrefabData, _configuration)
			//     .Init();

			Destroy(outerData.gameObject);
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