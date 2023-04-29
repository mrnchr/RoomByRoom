using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class OpenDoorSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<GameInfo> _gameInfo = default;
		private readonly EcsFilterInject<Inc<OpenDoorMessage>> _openDoorMsg = Idents.Worlds.MessageWorld;
		private readonly EcsFilterInject<Inc<Opener>> _opener = default;
		private EcsWorld _message;

		public void Run(IEcsSystems systems)
		{
			_message = systems.GetWorld(Idents.Worlds.MessageWorld);

			foreach (int index in _openDoorMsg.Value)
			foreach (int _ in _opener.Value)
			{
				if (IsFirstRoom(_gameInfo.Value.RoomCount))
					StartGame();

				CreateNextRoom();

				++_gameInfo.Value.RoomCount;
			}
		}

		private void CreateNextRoom()
		{
			_message.Add<NextRoomMessage>(_message.NewEntity())
				.Assign(x =>
				{
					x.Race.Type = FastRandom.GetEnemyRace();
					x.Room.Type = GetRoomType(_gameInfo.Value.RoomCount);
					return x;
				});
		}

		private void StartGame()
		{
			int startGameEntity = _message.NewEntity();
			_message.Add<StartGameMessage>(startGameEntity);
		}

		private static bool IsFirstRoom(int number) => number == 0;

		private static RoomType GetRoomType(int number) => number % 10 == 9 ? RoomType.Boss : RoomType.Enemy;
	}
}