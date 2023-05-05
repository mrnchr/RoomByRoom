using Leopotam.EcsLite;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class LoadGameSystem : IEcsRunSystem, IEcsInitSystem
	{
		private bool _isLoaded;
		private EcsWorld _message;

		public void Init(IEcsSystems systems)
		{
			_isLoaded = false;
			_message = systems.GetWorld(Idents.Worlds.MessageWorld);
		}

		public void Run(IEcsSystems systems)
		{
			if (_isLoaded) return;
			_message.Add<GameLoadedMessage>(_message.NewEntity());
			_isLoaded = true;
		}
	}
}