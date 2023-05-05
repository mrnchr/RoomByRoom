using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class PreparePlayerSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<NextRoomMessage>> _nextRoomMsgs = Idents.Worlds.MessageWorld;
		private readonly EcsFilterInject<Inc<Opener>> _openers = default;
		private EcsWorld _world;
		
		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach(int _ in _nextRoomMsgs.Value)
			foreach (int index in _openers.Value)
			{
				_world.Del<Opener>(index);
			}
		}
	}
}