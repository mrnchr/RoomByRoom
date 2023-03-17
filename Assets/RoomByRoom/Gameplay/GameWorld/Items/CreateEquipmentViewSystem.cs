using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    public class CreateEquipmentViewSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<InHands>, Exc<ItemViewRef>> _weapons = default;
        private EcsFilterInject<Inc<Equipped, ArmorInfo>, Exc<ItemViewRef>> _armors = default;
        private EcsCustomInject<PackedPrefabData> _prefabData = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            foreach(var index in _weapons.Value)
            {
                ref Owned owned = ref world.GetPool<Owned>().Get(index);
                ref UnitViewRef owner = ref world.GetPool<UnitViewRef>().Get(owned.Owner);
                ref WeaponInfo weaponInfo = ref world.GetPool<WeaponInfo>().Get(index);
                ref Shape shape = ref world.GetPool<Shape>().Get(index);

                GameObject weapon = GameObject.Instantiate(_prefabData.Value[ItemType.Weapon, (int)weaponInfo.Type, shape.PrefabIndex].gameObject);
                ItemView weaponView = weapon.GetComponent<ItemView>();
                weaponView.Entity = index;

                ref ItemViewRef weaponRef = ref world.GetPool<ItemViewRef>().Add(index);
                weaponRef.Value = weaponView;

                UnitView.ItemPlace place = owner.Value.Item;
                weaponView.transform.SetParent(place.Holder);
                weaponView.transform.position = place.Point.position;
                weaponView.transform.rotation = place.Point.rotation;
                weaponView.transform.localScale = place.Point.localScale;
            }

            foreach(var index in _armors.Value)
            {
                ref Owned owned = ref world.GetPool<Owned>().Get(index);
                ref UnitViewRef owner = ref world.GetPool<UnitViewRef>().Get(owned.Owner);
                ref ArmorInfo armorInfo = ref _armors.Pools.Inc2.Get(index);
                ref Shape shape = ref world.GetPool<Shape>().Get(index);

                GameObject armor = GameObject.Instantiate(_prefabData.Value[ItemType.Weapon, (int)armorInfo.Type, shape.PrefabIndex].gameObject);
                ItemView armorView = armor.GetComponent<ItemView>();
                armorView.Entity = index;

                ref ItemViewRef armorRef = ref world.GetPool<ItemViewRef>().Add(index);
                armorRef.Value = armorView;

                UnitView.ItemPlace place;

                if(owner.Value is HumanoidView humanoid)
                {
                    place = humanoid[armorInfo.Type];
                }
                else
                {
                    place = owner.Value.Item;
                }

                armorView.transform.SetParent(place.Holder);
                armorView.transform.position = place.Point.position;
                armorView.transform.rotation = place.Point.rotation;
                armorView.transform.localScale = place.Point.localScale;
            }
        }
    }
}