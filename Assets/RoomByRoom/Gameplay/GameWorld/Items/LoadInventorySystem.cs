using System;
using System.Collections.Generic;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    public class LoadInventorySystem : IEcsInitSystem
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
            void CollectEntity<T>(BoundComponent<T> component)
            where T : struct
            {
                _savedItems.Add(component.BoundEntity);
            }

            ProcessComponents(_savedInventory.Item, CollectEntity);
            ProcessComponents(_savedInventory.Weapon, CollectEntity);
            ProcessComponents(_savedInventory.PhysDamage, CollectEntity);
            ProcessComponents(_savedInventory.Protection, CollectEntity);
            ProcessComponents(_savedInventory.Equipped, CollectEntity);
            ProcessComponents(_savedInventory.Shape, CollectEntity);
        }

        void ProcessComponents<T>(List<BoundComponent<T>> components, Action<BoundComponent<T>> action)
        where T : struct
        {
            components.ForEach(action);
        }   

        private void LoadEntities()
        {
            void LoadComponent<T>(BoundComponent<T> component)
            where T : struct
            {
                ref T comp = ref _world.GetPool<T>().Add(_boundItems[component.BoundEntity]);
                comp = component.ComponentInfo;
            }
            
            ProcessComponents(_savedInventory.Item, LoadComponent);
            ProcessComponents(_savedInventory.Weapon, LoadComponent);
            ProcessComponents(_savedInventory.PhysDamage, LoadComponent);
            ProcessComponents(_savedInventory.Protection, LoadComponent);
            ProcessComponents(_savedInventory.Equipped, LoadComponent);
            ProcessComponents(_savedInventory.Shape, LoadComponent);
        }
    }
}