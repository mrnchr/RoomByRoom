using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom.Debugging
{
	public class MarkEnemySystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<Bonus>, Exc<CanBeDeleted>> _bonuses = default;
		private readonly EcsFilterInject<Inc<UnitViewRef>, Exc<ControllerByPlayer, CanBeDeleted>> _enemies = default;
		private readonly EcsFilterInject<Inc<ItemViewRef>> _items = default;
		private readonly EcsFilterInject<Inc<NextRoomMessage>> _nextRoom = Idents.Worlds.MessageWorld;
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

			foreach (int index in _enemies.Value)
				_world.Add<CanBeDeleted>(index);

			foreach (int index in _items.Value)
			{
				if (_world.Has<CanBeDeleted>(index))
				{
					if (IsPlayerWeapon(index))
						_world.Del<CanBeDeleted>(index);
				}
				else
				{
					if (!IsPlayerWeapon(index))
						_world.Add<CanBeDeleted>(index);
				}
			}

			foreach (int index in _bonuses.Value)
				_world.Add<CanBeDeleted>(index);
		}

		private bool IsPlayerWeapon(int entity) =>
			_world.Has<Owned>(entity)
			&& Utils.IsUnitOf(_world, _world.Get<Owned>(entity).Owner, UnitType.Player);
	}
}