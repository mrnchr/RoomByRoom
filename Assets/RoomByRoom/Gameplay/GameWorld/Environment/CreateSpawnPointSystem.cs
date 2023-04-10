using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class CreateSpawnPointSystem : IEcsRunSystem
	{
		private EcsFilterInject<Inc<AddPlayerCommand, RoomViewRef>> _rooms = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (var index in _rooms.Value)
			{
				SpawnPoint[] spawns = _world.GetComponent<RoomViewRef>(index).Value.SpawnPoints;
				Array.ForEach(spawns, CreateSpawnPointEntity);
			}
		}

		private void CreateSpawnPointEntity(SpawnPoint spawn)
		{
			int entity = _world.NewEntity();
			_world.AddComponent<SpawnPoint>(entity)
				.Assign(x => x = spawn);
		}
	}
}