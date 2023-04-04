using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class CreateEquipmentViewSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<Equipped>, Exc<ItemViewRef>> _equipments = default;
        private EcsCustomInject<PackedPrefabData> _prefabData = default;
        private EcsWorld _world;

        public void Run(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            foreach(var index in _equipments.Value)
            {
                ref ItemInfo itemInfo = ref _world.GetComponent<ItemInfo>(index);

                if (itemInfo.Type == ItemType.Artifact)
                    continue;

                View.InstantiateView(GetPrefab(index, itemInfo.Type), out ItemView itemView);
                itemView.Entity = index;

                _world.AddComponent<ItemViewRef>(index)
                    .Initialize(x => { x.Value = itemView; return x; });

                WearItem(index, itemInfo.Type, itemView);
            }
        }

        private void WearItem(int index, ItemType itemType, ItemView itemView)
        {
            ItemPlace place = GetPlace(index, itemType);
            itemView.transform.SetParent(place.Parent);
            Utils.SetTransform(itemView.transform, place.Point);
        }

        private GameObject GetPrefab(int entity, ItemType itemType)
        {
            int prefabIndex = 
                _world.GetComponent<Shape>(entity)
                .PrefabIndex;

            int typeNumber =
                itemType == ItemType.Weapon
                ? (int)_world.GetComponent<WeaponInfo>(entity).Type
                : (int)_world.GetComponent<ArmorInfo>(entity).Type;

            return _prefabData.Value.GetItem(itemType, typeNumber, prefabIndex)
                .gameObject;
        }

        private ItemPlace GetPlace(int entity, ItemType itemType)
        {
            HumanoidView humanoid = GetHumanoidUnit(entity);

            return itemType == ItemType.Weapon
                ? humanoid.GetWeaponPlace(_world.GetComponent<WeaponInfo>(entity).Type)
                : humanoid.GetArmorPlace(_world.GetComponent<ArmorInfo>(entity).Type);
        }

        private HumanoidView GetHumanoidUnit(int index)
        {
            return (HumanoidView)_world.GetComponent<UnitViewRef>(
                _world.GetComponent<Owned>(index).Owner)
                .Value;
        }
    }
}