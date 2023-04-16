using System;
using Leopotam.EcsLite;
using UnityEngine;
using Rand = UnityEngine.Random;

namespace RoomByRoom.Utility
{
	public static class FastRandom
	{
		public static RaceType GetEnemyRace() => (RaceType)Rand.Range(1, Utils.GetEnumLength<RaceType>());
		public static UnitType GetEnemyType() => (UnitType)Rand.Range(1, Utils.GetEnumLength<UnitType>() - 1);
		public static ItemType GetItemType() => (ItemType)Rand.Range(0, Utils.GetEnumLength<ItemType>());
		public static ArmorType GetArmorType() => (ArmorType)Rand.Range(0, Utils.GetEnumLength<ArmorType>() - 1);
		public static WeaponType GetWeaponType() => (WeaponType)Rand.Range(1, Utils.GetEnumLength<WeaponType>());

		public static float GetUnitHp(int roomCount, UnitType type)
		{
			float min = 1f, max = 2f;
			switch (type)
			{
				case UnitType.Baby:
					min = 1f;
					max = 1.5f;
					break;
				case UnitType.Flying:
					min = 1f;
					max = 1.75f;
					break;
				case UnitType.Humanoid:
					min = 1.25f;
					max = 1.75f;
					break;
				case UnitType.Giant:
					min = 1.5f;
					max = 2f;
					break;
				case UnitType.Boss:
					min = 2f;
					max = 2f;
					break;
			}

			return GetRandomFunctionValue(min, max, roomCount);
		}

		public static float GetArmorProtection(ArmorType type, int roomCount)
		{
			float min = 1f, max = 2f;
			switch (type)
			{
				case ArmorType.Boots:
					min = 1f;
					max = 1.5f;
					break;
				case ArmorType.BreastPlate:
					min = 1.5f;
					max = 2f;
					break;
				case ArmorType.Gloves:
					min = 1f;
					max = 1.5f;
					break;
				case ArmorType.Helmet:
					min = 1f;
					max = 2f;
					break;
				case ArmorType.Leggings:
					min = 1.25f;
					max = 1.75f;
					break;
			}

			return GetRandomFunctionValue(min, max, roomCount);
		}

		public static float GetPhysicalDamage(WeaponType type, int roomCount)
		{
			float min = 1f, max = 2f;
			switch (type)
			{
				case WeaponType.None:
					min = 1f / 2;
					max = 1.5f / 2;
					break;
				case WeaponType.Bow:
					min = 1.25f;
					max = 1.5f;
					break;
				case WeaponType.OneHand:
					min = 1.5f;
					max = 1.75f;
					break;
				case WeaponType.TwoHand:
					min = 1.75f;
					max = 2f;
					break;
			}

			return GetRandomFunctionValue(min, max, roomCount);
		}

		private static float GetRandomFunctionValue(float min, float max, int roomCount)
		{
			return Rand.Range(min, max) * roomCount * 10;
		}

		public static int GetEnemyRoom(int enemyRoomCount) => Rand.Range(0, enemyRoomCount);

		public static int CreateItem(EcsWorld world, PackedPrefabData prefabData, GameInfo gameInfo)
		{
			int item = world.NewEntity();

			// TODO: change to all item type
			ItemType type = (ItemType) Rand.Range(0, 2); // GetItemType();
			world.AddComponent<ItemInfo>(item)
				.Type = type;
			
			// TODO: create shield
			int equipmentType;
			if (type == ItemType.Armor)
			{
				equipmentType = (int)GetArmorType();
				world.AddComponent<ArmorInfo>(item)
					.Type = (ArmorType) equipmentType;
				
				world.AddComponent<ItemPhysicalProtection>(item)
					.Point = FastRandom.GetArmorProtection((ArmorType)equipmentType, gameInfo.RoomCount); 
			}
			else
			{
				// TODO: create all types of weapon
				equipmentType = (int)WeaponType.OneHand; // FastRandom.GetWeaponType();
				
				world.AddComponent<WeaponInfo>(item)
					.Type = (WeaponType) equipmentType;
				
				world.AddComponent<ItemPhysicalDamage>(item)
					.Point = FastRandom.GetPhysicalDamage((WeaponType) equipmentType, gameInfo.RoomCount);
			}

			world.AddComponent<Shape>(item)
				.PrefabIndex = GetPrefabIndex(prefabData, type, equipmentType);

			return item;
		}
		
		public static int GetPrefabIndex(PackedPrefabData prefabData, ItemType item, int equipmentType)
		{
			return Rand.Range(0, prefabData.GetItems(item, equipmentType).Length);
		}
	}
}