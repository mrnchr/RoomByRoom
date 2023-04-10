using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom.Debugging
{
	/// <summary>
	/// Remove marked (old) enemies when new room is spawning
	/// </summary>
	public class RemoveEntitySystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<UnitViewRef, CanBeDeleted>> _enemies = default;
		private readonly EcsFilterInject<Inc<ItemViewRef, CanBeDeleted>> _items = default;
		private readonly EcsFilterInject<Inc<NextRoomMessage>> _nextRoom = Idents.Worlds.MessageWorld;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			if (_nextRoom.Value.GetEntitiesCount() == 0)
				return;

			foreach (int index in _enemies.Value)
				DestroyEntityByView(_enemies.Pools.Inc1.Get(index).Value, index);

			foreach (int index in _items.Value)
				DestroyEntityByView(_items.Pools.Inc1.Get(index).Value, index);
		}

		private void DestroyEntityByView<TView>(TView view, int entity)
			where TView : View
		{
			UnityEngine.Object.Destroy(view.gameObject);
			_world.DelEntity(entity);
		}
	}
}