using System;
using System.Collections.Generic;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    internal class LoadInventorySystem : IEcsInitSystem
    {
        private EcsCustomInject<SavedData> _savedData = default;
        private EcsFilterInject<Inc<ControllerByPlayer>> _player = default;
        private HashSet<int> _savedItems = new HashSet<int>();
        private Dictionary<int, int> _boundItems = new Dictionary<int, int>();
        InventoryEntity _savedInventory;
        EcsWorld _world;

        public void Init(IEcsSystems systems)
        {
            if(_player.Value.GetEntitiesCount() == 0)
            {
                throw new TimeoutException("It is impossible to create inventory for the player. The entity does not exist");
            }

            _world = systems.GetWorld();
            _savedInventory = _savedData.Value.Inventory;

            // Collect all items which would be saved
            CollectEntities();

            // Create new entity for each item
            foreach(var item in _savedItems)
            {
                _boundItems[item] = _world.NewEntity();
            }

            // Add saved components for each item
            LoadEntities();

            // Assign the player as an owner to items
            foreach(var item in _boundItems)
            {
                ref Owned owned = ref _world.GetPool<Owned>().Add(item.Value);
                owned.Owner = _player.Value.GetRawEntities()[0];
            }
        }

        private void CollectEntities()
        {
            void CollectEntities<T>(List<BoundComponent<T>> components)
            where T : struct
            {
                foreach(var component in components)
                {
                    _savedItems.Add(component.BoundEntity);
                }
            }

            CollectEntities(_savedInventory.Item);
            CollectEntities(_savedInventory.Weapon);
            CollectEntities(_savedInventory.PhysDamage);
            CollectEntities(_savedInventory.Protection);
            CollectEntities(_savedInventory.Equipped);
        }

        private void LoadEntities()
        {
            void LoadEntities<T>(List<BoundComponent<T>> components)
            where T : struct
            {
                foreach(var component in components)
                {
                    ref T comp = ref _world.GetPool<T>().Add(_boundItems[component.BoundEntity]);
                    comp = component.ComponentInfo;
                }
            }

            LoadEntities(_savedInventory.Item);
            LoadEntities(_savedInventory.Weapon);
            LoadEntities(_savedInventory.PhysDamage);
            LoadEntities(_savedInventory.Protection);
            LoadEntities(_savedInventory.Equipped);
        }
    }
}