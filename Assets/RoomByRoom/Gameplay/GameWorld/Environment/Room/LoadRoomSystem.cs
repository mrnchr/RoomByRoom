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
			SavedRoom savedRoom = _savedData.Value.Room;

			int room = world.NewEntity();
			world.Add<RaceInfo>(room) = savedRoom.Race;
			world.Add<RoomInfo>(room) = savedRoom.Info;
		}
	}
}