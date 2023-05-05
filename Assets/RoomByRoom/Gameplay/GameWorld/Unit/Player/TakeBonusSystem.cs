using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.UI.Game;
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
		private readonly EcsCustomInject<KeepDirtyService> _keepDirtySvc = default;
		private readonly EcsCustomInject<CharacteristicService> _charSvc = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			int bonus = GetSelectedBonus();
			if (bonus == -1)
				return;

			int item = _world.Get<Bonus>(bonus).Item;
			foreach (int index in _takes.Value)
			{
				var inventory = _world.Get<Inventory>(index).ItemList;

				if (CanPutInEquipment(index, item))
				{
					_world.Add<Equipped>(item);
					var equipment = _world.Get<Equipment>(index).ItemList;
					equipment.Add(_world.PackEntity(item));
					_charSvc.Value.Calculate(index);

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
					_world.Get<Backpack>(index)
						.ItemList.Add(_world.PackEntity(item));
				}

				_world.Add<Owned>(item)
					.Owner = index;

				inventory.Add(_world.PackEntity(item));

				_keepDirtySvc.Value.UpdateDirtyMessage(DirtyType.Slots);

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
			Object.Destroy(_world.Get<BonusViewRef>(selected).Value.gameObject);
			_world.Del<ItemViewRef>(item);
			_world.DelEntity(selected);
		}

		private void ChangeMainWeapon(int hands, int item, int unit)
		{
			Object.Destroy(_world.Get<ItemViewRef>(hands).Value.gameObject);
			_world.Del<ItemViewRef>(hands);
			_world.Del<InHands>(hands);

			_world.Add<InHands>(item);
			_world.Get<MainWeapon>(unit)
				.Entity = item;

			Utils.SetWeaponToAnimate(_world, item, unit);
		}

		private bool IsMainWeapon(int unit, int weapon) => _world.Get<MainWeapon>(unit).Entity == weapon;

		private void Disarm(int hands, List<EcsPackedEntity> equipment, List<EcsPackedEntity> inventory)
		{
			if (hands == -1) return;
			_world.Del<Equipped>(hands);
			_world.Add<NotVisible>(hands);
			equipment.Remove(_world.PackEntity(hands));
			inventory.Remove(_world.PackEntity(hands));
		}

		private int GetHandsEntity(List<EcsPackedEntity> equipment)
		{
			foreach (int eq in equipment.Select(_world.Unpack).Where(IsHands))
				return eq;

			return -1;
		}

		private bool IsHands(int item) =>
			_world.Has<WeaponInfo>(item) && _world.Get<WeaponInfo>(item).Type == None;

		private bool IsListFull(List<EcsPackedEntity> list) => list.Count == list.Capacity;

		private bool IsBackPackFull(int entity) => IsListFull(_world.Get<Backpack>(entity).ItemList);

		private bool CanPutInEquipment(int entity, int checkedItem)
		{
			var list = _world.Get<Equipment>(entity).ItemList;
			if (IsListFull(list))
				return false;

			ItemTuple check = GetItemTuple(checkedItem);
			return list.Select(_world.Unpack)
				.Select(GetItemTuple)
				.All(x => !AreConflicted(x, check));
		}

		private static bool AreConflicted(ItemTuple placed, ItemTuple check) =>
			AreItemsSameType(placed, check) 
			|| AreMeleeWeapons(check, placed)
			|| AreTwoHandAndShield(check, placed);

		private static bool AreMeleeWeapons(ItemTuple a, ItemTuple b) => IsMeleeWeapon(a) && IsMeleeWeapon(b);

		private static bool AreItemsSameType(ItemTuple a, ItemTuple b) => a.Type == b.Type && a.Eq == b.Eq;

		private ItemTuple GetItemTuple(int item)
		{
			ItemType type = _world.Get<ItemInfo>(item).Type;
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
				Weapon => (int)_world.Get<WeaponInfo>(item).Type,
				Armor => (int)_world.Get<ArmorInfo>(item).Type,
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