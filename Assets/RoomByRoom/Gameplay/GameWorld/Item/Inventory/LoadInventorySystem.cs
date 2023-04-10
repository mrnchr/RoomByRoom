using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
	public class LoadInventorySystem : IEcsInitSystem
	{
		private readonly EcsFilterInject<Inc<ControllerByPlayer>> _player = default;
		private readonly EcsCustomInject<SavedData> _savedData = default;
		private readonly EcsCustomInject<CharacteristicService> _charSvc = default;
		private readonly HashSet<int> _savedItems = new();
		private readonly Dictionary<int, int> _boundItems = new();
		private InventoryEntity _savedInventory;
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
				_world.AddComponent<Owned>(itemEntity)
					.Assign(x =>
					{
						x.Owner = player;
						return x;
					});

				AddItemToInventory(player, itemEntity);
			}
			
			_charSvc.Value.Calculate(player);
			_world.GetComponent<UnitPhysicalProtection>(player)
				.Assign(x =>
				{
					if (x.CurrentPoint < 0)
						x.CurrentPoint = x.MaxPoint;
					return x;
				});
		}

		private void AddItemToInventory(int player, int item)
		{
			Utils.AddItemToList(_world.GetComponent<Inventory>(player).ItemList, item);
			Utils.AddItemToList(
				_world.HasComponent<Equipped>(item)
					? _world.GetComponent<Equipment>(player).ItemList
					: _world.GetComponent<Backpack>(player).ItemList, item);
		}

		private void CollectEntities()
		{
			void CollectEntity<T>(BoundComponent<T> component)
				where T : struct
			{
				_savedItems.Add(component.BoundEntity);
			}

			ProcessComponents(_savedInventory.Item, CollectEntity);
			ProcessComponents(_savedInventory.Weapon, CollectEntity);
			ProcessComponents(_savedInventory.Armor, CollectEntity);
			ProcessComponents(_savedInventory.PhysDamage, CollectEntity);
			ProcessComponents(_savedInventory.PhysProtection, CollectEntity);
			ProcessComponents(_savedInventory.Equipped, CollectEntity);
			ProcessComponents(_savedInventory.Shape, CollectEntity);
		}

		void ProcessComponents<T>(List<BoundComponent<T>> components, Action<BoundComponent<T>> action)
			where T : struct
		{
			components.ForEach(action);
		}

		private void LoadEntities()
		{
			void LoadComponent<T>(BoundComponent<T> component)
				where T : struct
			{
				_world.AddComponent<T>(_boundItems[component.BoundEntity])
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