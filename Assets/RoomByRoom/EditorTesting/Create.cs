using System.Collections.Generic;
using Leopotam.EcsLite;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom.Testing
{
	public static class Create
	{
		public static GameInfo GameInfo(int roomCount = 1)
		{
			return new GameInfo
			{
				RoomCount = roomCount
			};
		}
		
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

		public static ref Owned OwnedCmp(EcsWorld world, int armor, int unit)
		{
			return ref world.AddComponent<Owned>(armor)
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

		public static void EquippedCmp(EcsWorld world, int armor)
		{
			world.AddComponent<Equipped>(armor);
		}

		public static ref ItemPhysicalProtection ItemPhysicalProtection(EcsWorld world, int armor, int point = 0)
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
					Boots = new ArmorView[] { new GameObject().AddComponent<ArmorView>() },
					Leggings = new ArmorView[] { new GameObject().AddComponent<ArmorView>() },
					Breastplates = new ArmorView[] { new GameObject().AddComponent<ArmorView>() },
					Helmets = new ArmorView[] { new GameObject().AddComponent<ArmorView>() },
					Gloves = new ArmorView[] { new GameObject().AddComponent<ArmorView>() },
					OneHandWeapons = new WeaponView[] { new GameObject().AddComponent<WeaponView>() },
					Shields = new ArmorView[] { new GameObject().AddComponent<ArmorView>() }
				}
			};
		}

		public static ref Equipment EquipmentCmp(EcsWorld world, int enemy)
		{
			return ref world.AddComponent<Equipment>(enemy)
				.Assign(x =>
				{
					x.ItemList = new List<int>();
					return x;
				});
		}

		public static ref UnitViewRef UnitViewRefCmp(EcsWorld world, int entity, UnitView view = null)
		{
			if(!view)
				view = new GameObject().AddComponent<UnitView>();
			return ref world.AddComponent<UnitViewRef>(entity)
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
	}
}