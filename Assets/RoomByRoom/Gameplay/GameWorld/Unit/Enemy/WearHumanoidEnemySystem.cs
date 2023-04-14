using System.Collections.Generic;
using Rand = UnityEngine.Random;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class WearHumanoidEnemySystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<Bare>> _units = default;
		private readonly EcsCustomInject<GameInfo> _gameInfo = default;
		private readonly EcsCustomInject<PackedPrefabData> _prefabData = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (int index in _units.Value)
			{
				CreateArmors(index);
				CreateWeapon(index);
			}
		}

		private void CreateArmors(int entity)
		{
			int maxArmorNumber = Utils.GetEnumLength<ArmorType>() - 1;
			int armorNumber = Rand.Range(0, maxArmorNumber);
			List<int> armorTypes = new List<int>(new int[] { 0, 1, 2, 3, 4 });

			for (int i = 0; i < armorNumber; i++)
			{
				int index = Rand.Range(0, armorTypes.Count);
				CreateArmor((ArmorType)armorTypes[index], entity);

				armorTypes.RemoveAt(index);
			}
		}

		private void CreateArmor(ArmorType armorType, int unit)
		{
			int armor = CreateItemEntity(ItemType.Armor, (int)armorType, unit);

			_world.AddComponent<ArmorInfo>(armor)
				.Assign(x =>
				{
					x.Type = armorType;
					return x;
				});

			_world.AddComponent<ItemPhysicalProtection>(armor)
				.Assign(x =>
				{
					x.Point = FastRandom.GetArmorProtection(armorType, _gameInfo.Value.RoomCount);
					return x;
				});
		}

		private void CreateWeapon(int unit)
		{
			// TODO: change to random
			WeaponType weaponType = WeaponType.OneHand; // FastRandom.GetWeaponType();

			int weapon = CreateItemEntity(ItemType.Weapon, (int)weaponType, unit);

			_world.AddComponent<WeaponInfo>(weapon)
				.Assign(x =>
				{
					x.Type = weaponType;
					return x;
				});

			_world.AddComponent<ItemPhysicalDamage>(weapon)
				.Assign(x =>
				{
					x.Point = FastRandom.GetPhysicalDamage(weaponType, _gameInfo.Value.RoomCount);
					return x;
				});

			_world.AddComponent<InHands>(weapon);

			_world.AddComponent<MainWeapon>(unit)
				.Assign(x =>
				{
					x.Entity = weapon;
					return x;
				});

			if (weaponType == WeaponType.OneHand)
				CreateShield(unit);
		}

		// TODO: shield has persistent rather than protection
		private void CreateShield(int entity) => CreateArmor(ArmorType.Shield, entity);

		private int CreateItemEntity(ItemType itemType, int equipmentType, int unitEntity)
		{
			int item = _world.NewEntity();

			_world.AddComponent<ItemInfo>(item)
				.Assign(x =>
				{
					x.Type = itemType;
					return x;
				});

			_world.AddComponent<Equipped>(item);

			_world.AddComponent<Shape>(item)
				.Assign(x =>
				{
					x.PrefabIndex = GetRandomPrefabIndex(itemType, equipmentType);
					return x;
				});

			_world.AddComponent<Owned>(item)
				.Assign(x =>
				{
					x.Owner = unitEntity;
					return x;
				});

			return item;
		}

		private int GetRandomPrefabIndex(ItemType item, int equipmentType)
		{
			return Rand.Range(0, _prefabData.Value.GetItems(item, equipmentType).Length);
		}
	}
}