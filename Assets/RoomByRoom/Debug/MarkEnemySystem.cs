using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom.Debugging
{
	public class MarkEnemySystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<NextRoomMessage>> _nextRoom = Idents.Worlds.MessageWorld;
		private readonly EcsFilterInject<Inc<UnitViewRef>, Exc<ControllerByPlayer, CanBeDeleted>> _enemies = default;
		private readonly EcsFilterInject<Inc<ItemViewRef>, Exc<CanBeDeleted>> _items = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			// Add CanBeDeleted component for all enemies. This component
			// is needed to delete these enemies on the next room's spawn
			// We do it when next room message is out because when next room 
			// message is send and this script is executed new and old
			// enemies are same

			if (_nextRoom.Value.GetEntitiesCount() > 0)
				return;

			_world = systems.GetWorld();

			foreach (int enemy in _enemies.Value)
				_world.AddComponent<CanBeDeleted>(enemy);

			foreach (int index in _items.Value)
			{
				if (!IsPlayerWeapon(index))
					_world.AddComponent<CanBeDeleted>(index);
			}
		}

		private bool IsPlayerWeapon(int entity)
		{
			return _world.GetComponent<Owned>(entity).Owner ==
			       _world.Filter<ControllerByPlayer>().End().GetRawEntities()[0];
		}
	}
}