using System;
using System.Collections.Generic;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class LoadInventorySystem : IEcsInitSystem
    {
        private EcsCustomInject<SavedData> _savedData = default;
        private EcsFilterInject<Inc<ControllerByPlayer>> _player = default;
        private HashSet<int> _savedItems = new();
        private Dictionary<int, int> _boundItems = new();
        private InventoryEntity _savedInventory;
        private EcsWorld _world;

        public void Init(IEcsSystems systems)
        {
            if(_player.Value.GetEntitiesCount() == 0)
                throw new TimeoutException("It is impossible to create inventory for the player. The entity does not exist");

            _world = systems.GetWorld();
            _savedInventory = _savedData.Value.Inventory;

            CollectEntities();

            foreach(int item in _savedItems)
                _boundItems[item] = _world.NewEntity();

            LoadEntities();

            foreach(int itemEntity in _boundItems.Values)
            {
                _world.AddComponent<Owned>(itemEntity)
                    .Initialize(x => { x.Owner = _player.Value.GetRawEntities()[0]; return x; });
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
            ProcessComponents(_savedInventory.Armor, CollectEntity);
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
                _world.AddComponent<T>(_boundItems[component.BoundEntity])
                    .Initialize(x => x = component.ComponentInfo);
            }

            ProcessComponents(_savedInventory.Item, LoadComponent);
            ProcessComponents(_savedInventory.Weapon, LoadComponent);
            ProcessComponents(_savedInventory.Armor, LoadComponent);
            ProcessComponents(_savedInventory.PhysDamage, LoadComponent);
            ProcessComponents(_savedInventory.Protection, LoadComponent);
            ProcessComponents(_savedInventory.Equipped, LoadComponent);
            ProcessComponents(_savedInventory.Shape, LoadComponent);
        }
    }
}