using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using static RoomByRoom.ArmorType;
using static RoomByRoom.ItemType;
using static RoomByRoom.WeaponType;
using Object = UnityEngine.Object;

namespace RoomByRoom
{
	public class TakeBonusSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<Selected>> _bonus = default;
		private readonly EcsFilterInject<Inc<TakeCommand>> _takes = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			int bonus = GetSelectedBonus();
			if (bonus == -1)
				return;

			int item = _world.GetComponent<Bonus>(bonus).Item;
			foreach (int index in _takes.Value)
			{
				var inventory = _world.GetComponent<Inventory>(index).ItemList;

				if (CanPutInEquipment(index, item))
				{
					_world.AddComponent<Equipped>(item);
					var equipment = _world.GetComponent<Equipment>(index).ItemList;
					equipment.Add(item);

					if (IsMeleeWeapon(GetItemTuple(item)))
					{
						int hands = GetHandsEntity(equipment);
						Disarm(hands, equipment, inventory);

						if (IsMainWeapon(index, hands))
							ChangeMainWeapon(hands, item, index);
					}
				}
				else
				{
					if (IsBackPackFull(index))
						continue;
					_world.GetComponent<Backpack>(index)
						.ItemList.Add(item);
				}

				_world.AddComponent<Owned>(item)
					.Owner = index;

				inventory.Add(item);

				DestroyBonus(bonus, item);
			}
		}

		private int GetSelectedBonus()
		{
			int selected = -1;
			foreach (int index in _bonus.Value)
				selected = index;
			return selected;
		}

		private void DestroyBonus(int selected, int item)
		{
			Object.Destroy(_world.GetComponent<BonusViewRef>(selected).Value.gameObject);
			_world.DelComponent<ItemViewRef>(item);
			_world.DelEntity(selected);
		}

		private void ChangeMainWeapon(int hands, int item, int index)
		{
			_world.GetComponent<ItemViewRef>(hands).Value.gameObject.SetActive(false);
			_world.DelComponent<InHands>(hands);

			_world.AddComponent<InHands>(item);
			_world.GetComponent<MainWeapon>(index)
				.Entity = item;
		}

		private bool IsMainWeapon(int unit, int weapon) => _world.GetComponent<MainWeapon>(unit).Entity == weapon;

		private void Disarm(int hands, List<int> equipment, List<int> inventory)
		{
			if (hands != -1)
			{
				_world.DelComponent<Equipped>(hands);
				_world.AddComponent<NotVisible>(hands);
				equipment.Remove(hands);
				inventory.Remove(hands);
			}
		}

		private int GetHandsEntity(List<int> equipment)
		{
			foreach (int eq in equipment)
			{
				if (IsHands(eq))
					return eq;
			}

			return -1;
		}

		private bool IsHands(int item) =>
			_world.HasComponent<WeaponInfo>(item) && _world.GetComponent<WeaponInfo>(item).Type == None;

		private bool IsListFull(List<int> list) => list.Count == list.Capacity;

		private bool IsBackPackFull(int entity) => IsListFull(_world.GetComponent<Backpack>(entity).ItemList);

		private bool CanPutInEquipment(int entity, int checkedItem)
		{
			var list = _world.GetComponent<Equipment>(entity).ItemList;
			if (IsListFull(list))
				return false;

			ItemTuple check = GetItemTuple(checkedItem);
			foreach (int index in list)
			{
				ItemTuple placed = GetItemTuple(index);
				// Debug.Log($"for {checkedItem} and {index} output: {AreConflicted(placed, check)}");
				if (AreConflicted(placed, check))
					return false;
			}

			return true;
		}

		private static bool AreConflicted(ItemTuple placed, ItemTuple check) => AreItemsSameType(placed, check) ||
		                                                                        AreMeleeWeapons(check, placed) ||
		                                                                        AreTwoHandAndShield(check, placed);

		private static bool AreMeleeWeapons(ItemTuple a, ItemTuple b) =>
			a.Type == Weapon && b.Type == Weapon && IsMeleeWeapon(a) && IsMeleeWeapon(b);

		private static bool AreItemsSameType(ItemTuple a, ItemTuple b) => a.Type == b.Type && a.Eq == b.Eq;

		private ItemTuple GetItemTuple(int item)
		{
			ItemType type = _world.GetComponent<ItemInfo>(item).Type;
			return new ItemTuple
			{
				Type = type,
				Eq = GetEquipmentType(item, type)
			};
		}

		private static bool AreTwoHandAndShield(ItemTuple a, ItemTuple b) =>
			(IsTwoHand(a) && IsShield(b)) || (IsTwoHand(b) && IsShield(a));

		private static bool IsShield(ItemTuple item) => item.Type == Armor && (ArmorType)item.Eq == Shield;

		private static bool IsTwoHand(ItemTuple item) => item.Type == Weapon && (WeaponType)item.Eq == TwoHands;

		private static bool IsMeleeWeapon(ItemTuple item) =>
			item.Type == Weapon && ((WeaponType)item.Eq == OneHand || (WeaponType)item.Eq == TwoHands);

		private int GetEquipmentType(int item, ItemType itemType)
		{
			// TODO: add artifact
			return itemType switch
			{
				Weapon => (int)_world.GetComponent<WeaponInfo>(item).Type,
				Armor => (int)_world.GetComponent<ArmorInfo>(item).Type,
				_ => throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null)
			};
		}

		private struct ItemTuple
		{
			public ItemType Type;
			public int Eq;
		}
	}
}