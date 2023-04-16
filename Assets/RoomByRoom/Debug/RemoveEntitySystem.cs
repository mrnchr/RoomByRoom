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
		private readonly EcsFilterInject<Inc<CanBeDeleted>> _deleted = default;
		private readonly EcsFilterInject<Inc<NextRoomMessage>> _nextRoom = Idents.Worlds.MessageWorld;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			if (_nextRoom.Value.GetEntitiesCount() == 0)
				return;

			foreach (int index in _deleted.Value)
			{
				if(TryGetView(index, out View view))
					UnityEngine.Object.Destroy(view.gameObject);
				_world.DelEntity(index);
			}
		}

		private bool TryGetView(int entity, out View view)
		{
			view = null;
			if (_world.HasComponent<UnitViewRef>(entity))
				return view = _world.GetComponent<UnitViewRef>(entity).Value;
			
			if (_world.HasComponent<ItemViewRef>(entity))
				return view = _world.GetComponent<ItemViewRef>(entity).Value;
			
			if (_world.HasComponent<BonusViewRef>(entity))
				return view = _world.GetComponent<BonusViewRef>(entity).Value;

			return false;
		}
	}
}