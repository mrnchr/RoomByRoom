using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class LoadInventorySystem : IEcsInitSystem
	{
		private readonly Dictionary<int, int> _boundItems = new Dictionary<int, int>();
		private readonly EcsCustomInject<CharacteristicService> _charSvc = default;
		private readonly EcsFilterInject<Inc<ControllerByPlayer>> _player = default;
		private readonly EcsCustomInject<Saving> _savedData = default;
		private readonly HashSet<int> _savedItems = new HashSet<int>();
		private SavedInventory _savedInventory;
		private EcsWorld _world;

		public void Init(IEcsSystems systems)
		{
			if (_player.Value.GetEntitiesCount() == 0)
				throw new TimeoutException("It is impossible to create inventory for the player. The entity does not exist");
			
			_world = systems.GetWorld();
			_savedInventory = _savedData.Value.Inventory;

			CollectEntities();

			foreach (int item in _savedItems)
				_boundItems[item] = _world.NewEntity();

			LoadEntities();

			int player = _player.Value.GetRawEntities()[0];
			foreach (int itemEntity in _boundItems.Values)
			{
				_world.Add<Owned>(itemEntity)
					.Owner = player;

				AddItemToInventory(player, itemEntity);
			}

			_charSvc.Value.Calculate(player);
			_world.Get<UnitPhysicalProtection>(player)
				.Assign(x =>
				{
					x.CurrentPoint.Clamp(max: x.MaxPoint);
					return x;
				});

			Utils.SendDirtyMessage(systems.GetWorld(Idents.Worlds.MessageWorld));
		}

		private void AddItemToInventory(int player, int item)
		{
			Utils.AddItemToList(_world.Get<Inventory>(player).ItemList, item);
			Utils.AddItemToList(
				_world.Has<Equipped>(item)
					? _world.Get<Equipment>(player).ItemList
					: _world.Get<Backpack>(player).ItemList, item);
		}

		private void CollectEntities()
		{
			void CollectEntity<T>(BoundComponent<T> component)
				where T : struct =>
				_savedItems.Add(component.BoundEntity);

			ProcessComponents(_savedInventory.Item, CollectEntity);
			ProcessComponents(_savedInventory.Weapon, CollectEntity);
			ProcessComponents(_savedInventory.Armor, CollectEntity);
			ProcessComponents(_savedInventory.PhysDamage, CollectEntity);
			ProcessComponents(_savedInventory.PhysProtection, CollectEntity);
			ProcessComponents(_savedInventory.Equipped, CollectEntity);
			ProcessComponents(_savedInventory.Shape, CollectEntity);
		}

		private void ProcessComponents<T>(List<BoundComponent<T>> components, Action<BoundComponent<T>> action)
			where T : struct =>
			components.ForEach(action);

		private void LoadEntities()
		{
			void LoadComponent<T>(BoundComponent<T> component)
				where T : struct
			{
				_world.Add<T>(_boundItems[component.BoundEntity])
					.Assign(x => component.ComponentInfo);
			}

			ProcessComponents(_savedInventory.Item, LoadComponent);
			ProcessComponents(_savedInventory.Weapon, LoadComponent);
			ProcessComponents(_savedInventory.Armor, LoadComponent);
			ProcessComponents(_savedInventory.PhysDamage, LoadComponent);
			ProcessComponents(_savedInventory.PhysProtection, LoadComponent);
			ProcessComponents(_savedInventory.Equipped, LoadComponent);
			ProcessComponents(_savedInventory.Shape, LoadComponent);
		}
	}
}