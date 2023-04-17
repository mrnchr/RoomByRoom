using System.Collections.Generic;
using Leopotam.EcsLite;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom.Testing
{
	public static class Create
	{
		public static GameInfo GameInfo(int roomCount = 1) =>
			new GameInfo
			{
				RoomCount = roomCount
			};

		public static ref GetDamageMessage GetDamageMessageCmp(EcsWorld message, int entity, int unit, int weapon)
		{
			return ref message.AddComponent<GetDamageMessage>(entity)
				.Assign(x =>
				{
					x.Damaged = unit;
					x.Weapon = weapon;
					return x;
				});
		}

		public static ref UnitPhysicalProtection UnitPhysicalProtectionCmp(EcsWorld world,
			int unit,
			float currPoint = 0,
			float maxPoint = 0,
			float restoreSpeed = 0,
			float cantRestoreTime = 0)
		{
			return ref world.AddComponent<UnitPhysicalProtection>(unit)
				.Assign(x =>
				{
					x.MaxPoint = maxPoint;
					x.CurrentPoint = currPoint;
					x.RestoreSpeed = restoreSpeed;
					x.CantRestoreTime = cantRestoreTime;
					return x;
				});
		}

		public static ref Health HealthCmp(EcsWorld world, int unit, float currPoint = 0, float maxPoint = 0)
		{
			return ref world.AddComponent<Health>(unit)
				.Assign(x =>
				{
					x.CurrentPoint = currPoint;
					x.MaxPoint = maxPoint;
					return x;
				});
		}

		public static ref ItemPhysicalDamage ItemPhysicalDamageCmp(EcsWorld world, int weapon, float point = 0)
		{
			return ref world.AddComponent<ItemPhysicalDamage>(weapon)
				.Assign(x =>
				{
					x.Point = point;
					return x;
				});
		}

		public static ref Owned OwnedCmp(EcsWorld world, int item, int unit)
		{
			return ref world.AddComponent<Owned>(item)
				.Assign(x =>
				{
					x.Owner = unit;
					return x;
				});
		}

		public static ref ArmorInfo ArmorInfoCmp(EcsWorld world, int armor, ArmorType type = ArmorType.Boots)
		{
			return ref world.AddComponent<ArmorInfo>(armor)
				.Assign(x =>
				{
					x.Type = type;
					return x;
				});
		}

		public static ref WeaponInfo WeaponInfoCmp(EcsWorld world, int weapon, WeaponType type = WeaponType.OneHand)
		{
			return ref world.AddComponent<WeaponInfo>(weapon)
				.Assign(x =>
				{
					x.Type = type;
					return x;
				});
		}

		public static void EquippedCmp(EcsWorld world, int armor)
		{
			world.AddComponent<Equipped>(armor);
		}

		public static ref ItemPhysicalProtection ItemPhysicalProtectionCmp(EcsWorld world, int armor, float point = 0)
		{
			return ref world.AddComponent<ItemPhysicalProtection>(armor)
				.Assign(x =>
				{
					x.Point = point;
					return x;
				});
		}

		public static ref UnitInfo UnitInfoCmp(EcsWorld world, int unit, UnitType type = UnitType.Humanoid)
		{
			return ref world.AddComponent<UnitInfo>(unit)
				.Assign(x =>
				{
					x.Type = type;
					return x;
				});
		}

		public static ref Bonus BonusCmp(EcsWorld world, int entity, int item)
		{
			return ref world.AddComponent<Bonus>(entity)
				.Assign(x =>
				{
					x.Item = item;
					return x;
				});
		}

		public static ref ItemInfo ItemInfoCmp(EcsWorld world, int item, ItemType type = ItemType.Armor)
		{
			return ref world.AddComponent<ItemInfo>(item)
				.Assign(x =>
				{
					x.Type = type;
					return x;
				});
		}

		public static ref CantRestore CantRestoreCmp(EcsWorld world, int unit, float timeLeft = 0)
		{
			return ref world.AddComponent<CantRestore>(unit)
				.Assign(x =>
				{
					x.TimeLeft = timeLeft;
					return x;
				});
		}

		public static PackedPrefabData PackedPrefabData()
		{
			return new PackedPrefabData(ScriptableObject.CreateInstance<PrefabData>())
			{
				Prefabs =
				{
					Boots = new[] { new GameObject().AddComponent<ArmorView>() },
					Leggings = new[] { new GameObject().AddComponent<ArmorView>() },
					Breastplates = new[] { new GameObject().AddComponent<ArmorView>() },
					Helmets = new[] { new GameObject().AddComponent<ArmorView>() },
					Gloves = new[] { new GameObject().AddComponent<ArmorView>() },
					OneHandWeapons = new[] { Setup.WeaponViewF(GO<WeaponView>()) },
					Shields = new[] { new GameObject().AddComponent<ArmorView>() }
				}
			};
		}

		public static ref Equipment EquipmentCmp(EcsWorld world, int entity, int size = 16, params int[] items)
		{
			return ref world.AddComponent<Equipment>(entity)
				.Assign(x =>
				{
					x.ItemList = new List<int>(size);
					x.ItemList.AddRange(items);
					return x;
				});
		}

		public static ref Inventory InventoryCmp(EcsWorld world, int entity, int size = 26, params int[] items)
		{
			return ref world.AddComponent<Inventory>(entity)
				.Assign(x =>
				{
					x.ItemList = new List<int>(size);
					x.ItemList.AddRange(items);
					return x;
				});
		}

		public static ref Backpack BackpackCmp(EcsWorld world, int entity, int size = 10, params int[] items)
		{
			return ref world.AddComponent<Backpack>(entity)
				.Assign(x =>
				{
					x.ItemList = new List<int>(size);
					x.ItemList.AddRange(items);
					return x;
				});
		}

		public static ref UnitViewRef UnitViewRefCmp(EcsWorld world, int entity, UnitView view = null)
		{
			if (!view)
				view = new GameObject().AddComponent<UnitView>();
			return ref world.AddComponent<UnitViewRef>(entity)
				.Assign(x =>
				{
					x.Value = view;
					return x;
				});
		}

		public static ref BonusViewRef BonusViewRefCmp(EcsWorld world, int entity, BonusView view = null)
		{
			if (!view)
				view = new GameObject().AddComponent<BonusView>();
			return ref world.AddComponent<BonusViewRef>(entity)
				.Assign(x =>
				{
					x.Value = view;
					return x;
				});
		}

		public static ref ItemViewRef ItemViewRefCmp(EcsWorld world, int index, ItemView view = null)
		{
			if (!view)
				view = new GameObject().AddComponent<ItemView>();
			return ref world.AddComponent<ItemViewRef>(index)
				.Assign(x =>
				{
					x.Value = view;
					return x;
				});
		}

		public static PlayerData PlayerData(int takeItemDistance = 2)
		{
			var playerData = ScriptableObject.CreateInstance<PlayerData>();
			playerData.TakeItemDistance = takeItemDistance;
			return playerData;
		}

		public static T GO<T>()
			where T : MonoBehaviour =>
			new GameObject().AddComponent<T>();

		public static ref Shape ShapeCmp(EcsWorld world, int item, int shape)
		{
			return ref world.AddComponent<Shape>(item)
				.Assign(x =>
				{
					x.PrefabIndex = shape;
					return x;
				});
		}

		public static void InHandsCmp(EcsWorld world, int item) => world.AddComponent<InHands>(item);
	}
}