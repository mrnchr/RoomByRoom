using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
	public class RecreateRoomSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<NextRoomMessage>> _nextRoomMsg = Idents.Worlds.MessageWorld;
		private readonly EcsFilterInject<Inc<RoomViewRef>> _room = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			EcsWorld message = systems.GetWorld(Idents.Worlds.MessageWorld);

			foreach (int index in _nextRoomMsg.Value)
			{
				DeleteRoom();
				CreateRoom(message.GetComponent<NextRoomMessage>(index));
			}
		}

		private void CreateRoom(NextRoomMessage nextRoom)
		{
			int roomEntity = _world.NewEntity();

			_world.AddComponent<RaceInfo>(roomEntity)
				.Assign(x => x = nextRoom.Race);

			_world.AddComponent<RoomInfo>(roomEntity)
				.Assign(x => x = nextRoom.Room);
		}

		private void DeleteRoom()
		{
			int roomEntity = _room.Value.GetRawEntities()[0];
			Object.Destroy(_world.GetComponent<RoomViewRef>(roomEntity).Value.gameObject);
			_world.DelEntity(roomEntity);
		}
	}
}