using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	internal class LoadRoomSystem : IEcsInitSystem
	{
		private readonly EcsCustomInject<Saving> _savedData = default;

		public void Init(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();

			int room = world.NewEntity();
			RoomEntity roomEntity = _savedData.Value.Room;

			world.AddComponent<RaceInfo>(room)
				.Assign(x => x = roomEntity.Race);

			world.AddComponent<RoomInfo>(room)
				.Assign(x => x = roomEntity.Info);
		}
	}
}