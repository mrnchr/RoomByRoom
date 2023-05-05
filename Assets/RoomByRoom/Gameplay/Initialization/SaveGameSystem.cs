using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.UI.Game.Inventory;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
	public class SaveGameSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<PlayerDyingMessage>> _dieMsgs = Idents.Worlds.MessageWorld;
		private readonly EcsFilterInject<Inc<RoomCleanedMessage>> _cleanedMsgs = Idents.Worlds.MessageWorld;
		private readonly EcsFilterInject<Inc<InventoryChangedMessage>> _changedMsgs = Idents.Worlds.MessageWorld;
		private readonly EcsFilterInject<Inc<NextRoomMessage>> _nextRoomMsgs = Idents.Worlds.MessageWorld;
		private readonly EcsCustomInject<InitializeData> _defaultData = default;
		private readonly EcsCustomInject<Saving> _savedData = default;
		private readonly EcsCustomInject<SavingService> _savingSvc = default;
		private readonly EcsCustomInject<GameInfo> _gameInfo = default;
		private EcsWorld _world;
		private bool _canSave;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (int _ in _nextRoomMsgs.Value)
				_canSave = false;
			
			foreach (int _ in _cleanedMsgs.Value)
			{
				_canSave = true;
				PackSaving();
				_savingSvc.Value.SaveData(_savedData.Value);
			}

			if(_changedMsgs.Value.GetEntitiesCount() > 0 && _canSave)
			{
				PackSaving();
				_savingSvc.Value.SaveData(_savedData.Value);
			}

			foreach (int index in _changedMsgs.Value)
				_changedMsgs.Pools.Inc1.Del(index);

			foreach (int _ in _dieMsgs.Value)				
			{
				SetDefaultData();
				_savingSvc.Value.SaveData(_savedData.Value);
			}
		}

		private void PackSaving()
		{
			int player = Utils.GetPlayerEntity(_world);
			_savedData.Value.GameSave = _gameInfo.Value;
			_savedData.Value.Player = new SavedPlayer
			{
				Race = _world.Get<RaceInfo>(player),
				HealthCmp = _world.Get<Health>(player),
				MovableCmp = _world.Get<Movable>(player),
				JumpableCmp = _world.Get<Jumpable>(player),
				UnitPhysProtectionCmp = _world.Get<UnitPhysicalProtection>(player)
			};

			int room = Utils.GetRoomEntity(_world);
			_savedData.Value.Room = new SavedRoom
			{
				Info = _world.Get<RoomInfo>(room),
				Race = _world.Get<RaceInfo>(room)
			};

			var playerItems = _world.Get<Inventory>(player).ItemList.Select(x => _world.Unpack(x)).ToList();
			EcsFilter handFilter = _world.Filter<NotVisible>().End(1);
			if (handFilter.GetEntitiesCount() > 0)
				playerItems.Add(handFilter.GetRawEntities()[0]);

			_savedData.Value.Inventory = new SavedInventory
			{
				Item = SelectComponents<ItemInfo>(playerItems),
				Weapon = SelectComponents<WeaponInfo>(playerItems),
				Armor = SelectComponents<ArmorInfo>(playerItems),
				PhysDamage = SelectComponents<ItemPhysicalDamage>(playerItems),
				PhysProtection = SelectComponents<ItemPhysicalProtection>(playerItems),
				Equipped = SelectComponents<Equipped>(playerItems),
				Shape = SelectComponents<Shape>(playerItems),
			};
		}

		private List<BoundComponent<TComponent>> SelectComponents<TComponent>(IEnumerable<int> playerItems)
			where TComponent: struct => 
			(from index in playerItems
				where _world.Has<TComponent>(index)
				select new BoundComponent<TComponent>()
					{ BoundEntity = index, ComponentInfo = _world.Get<TComponent>(index) }).ToList();

		private void SetDefaultData()
		{
			_savedData.Value.GameSave = _defaultData.Value.GameInfo;
			_savedData.Value.Player = _defaultData.Value.Player;
			_savedData.Value.Room = _defaultData.Value.Room;
			_savedData.Value.Inventory = _defaultData.Value.Inventory;
		}
	}
}