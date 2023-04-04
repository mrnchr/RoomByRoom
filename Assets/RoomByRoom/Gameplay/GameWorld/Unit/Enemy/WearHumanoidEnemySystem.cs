using System.Collections.Generic;
using Rand = UnityEngine.Random;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class WearHumanoidEnemySystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<Bare>> _units = default;
        private EcsCustomInject<GameInfo> _gameInfo = default;
        private EcsCustomInject<PackedPrefabData> _prefabData = default;
        private EcsWorld _world;

        public void Run(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            foreach(int index in _units.Value)
            {
                CreateArmors(index);
                CreateWeapon(index);

                _world.DelComponent<Bare>(index);
            }
        }

        private void CreateArmors(int entity)
        {
            int maxArmorNumber = Utils.GetEnumLength<ArmorType>() - 1;
            int armorNumber = Rand.Range(0, maxArmorNumber);
            List<int> armorTypes = new List<int>(new int[] {0, 1, 2, 3, 4});

            for (int i = 0; i < armorNumber; i++)
            {
                int index = Rand.Range(0, armorTypes.Count);
                CreateArmor((ArmorType)armorTypes[index], entity);

                armorTypes.RemoveAt(index);
            }
        }

        private void CreateArmor(ArmorType armorType, int entity)
        {
            int armor = CreateItemEntity(ItemType.Armor, (int)armorType, entity);

            _world.AddComponent<ArmorInfo>(armor)
                .Initialize(x => { x.Type = armorType; return x; });

            _world.AddComponent<Protection>(armor)
                .Initialize(x =>
                {
                    x.Point = FastRandom.GetArmorProtection(armorType, _gameInfo.Value.RoomCount);
                    return x;
                });
        }

        private void CreateWeapon(int entity)
        {
            int maxWeaponNumber = Utils.GetEnumLength<WeaponType>() - 1;
            WeaponType weaponType = (WeaponType)Rand.Range(1, maxWeaponNumber);

            int weapon = CreateItemEntity(ItemType.Weapon, (int)weaponType, entity);

            _world.AddComponent<WeaponInfo>(weapon)
                .Initialize(x => { x.Type = weaponType; return x; });

            _world.AddComponent<PhysicalDamage>(weapon)
                .Initialize(x =>
                {
                    x.Point = FastRandom.GetPhysicalDamage(weaponType, _gameInfo.Value.RoomCount);
                    return x;
                });

            _world.AddComponent<InHands>(weapon);

            _world.AddComponent<MainWeapon>(entity)
                .Initialize(x => { x.Entity = weapon; return x; });

            if(weaponType == WeaponType.OneHand)
                CreateShield(entity);
        }

        // TODO: shield has persistent rather than protection
        private void CreateShield(int entity) => CreateArmor(ArmorType.Shield, entity);

        private int CreateItemEntity(ItemType itemType, int equipmentType, int unitEntity)
        {
            int item = _world.NewEntity();

            _world.AddComponent<ItemInfo>(item)
                .Initialize(x => { x.Type = itemType; return x; });

            _world.AddComponent<Equipped>(item);

            _world.AddComponent<Shape>(item)
                .Initialize(x =>
                {
                    x.PrefabIndex = GetRandomPrefabIndex(itemType, equipmentType);
                    return x;
                });

            _world.AddComponent<Owned>(item)
                .Initialize(x => { x.Owner = unitEntity; return x; });

            return item;
        }

        int GetRandomPrefabIndex(ItemType item, int equipmentType)
        {
            return Rand.Range(0, _prefabData.Value.GetItems(item, equipmentType).Length);
        }
    }
}