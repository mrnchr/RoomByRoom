using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom.UI.Game.Inventory
{
	public class InventoryKeeper : MonoBehaviour
	{
		[SerializeField] private List<Slot> _inventory;
		private List<Slot> _backpack;
		private List<Slot> _equipment;
		private EcsWorld _world;
		private SpriteService _spriteSvc;

		private void Awake()
		{
			_equipment = _inventory.Where(x => x.Info.IsEquipped).ToList();
			_backpack = _inventory.Where(x => !x.Info.IsEquipped).ToList();
		}

		public void Construct(EcsWorld world, SpriteService spriteSvc)
		{
			_world = world;
			_spriteSvc = spriteSvc;
		}

		public void UpdateInventory(List<int> inventory)
		{
			_inventory.ForEach(x => x.SetItem(null));

			foreach (int index in inventory)
			{
				ItemType type = _world.Get<ItemInfo>(index).Type;
				int eqType = GetEqType(index, type);
				int slotEqType = GetSlotEquipmentType(index, type);
				Slot slot = GetSlot(index, type, slotEqType);
				Debug.Log($"index: {index}, type: {type}, eqType: {slotEqType}");
				int shape = _world.Get<Shape>(index).PrefabIndex;
				Debug.Log(slot ? slot.name : slot);
				slot.SetItem(_spriteSvc.GetItem(type, eqType, shape));
			}
		}

		private int GetEqType(int item, ItemType type) =>
			type switch
			{
				ItemType.Weapon => (int)_world.Get<WeaponInfo>(item).Type,
				ItemType.Armor => (int)_world.Get<ArmorInfo>(item).Type,
				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};

		private Slot GetSlot(int item, ItemType type, int eqType)
		{
			var emptySlots = _inventory.FindAll(x => x.IsEmpty);
			
			if (!_world.Has<Equipped>(item))
				return emptySlots.Find(x => !x.Info.IsEquipped);
			
			return type == ItemType.Weapon
				? emptySlots.Find(x => x.Info.WeaponInfo != null && x.Info.WeaponInfo.Type == (WeaponSlotType)eqType)
				: emptySlots.Find(x => x.Info.ArmorInfo != null && x.Info.ArmorInfo.Type == (ArmorType)eqType);
		}

		private int GetSlotEquipmentType(int item, ItemType type) =>
			type switch
			{
				ItemType.Weapon when _world.Get<WeaponInfo>(item).Type == WeaponType.Bow => (int)WeaponSlotType.Bow,
				ItemType.Weapon => (int)WeaponSlotType.Melee,
				ItemType.Armor => (int)_world.Get<ArmorInfo>(item).Type,
				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
	}
}