using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using Object = UnityEngine.Object;

namespace RoomByRoom
{
	internal class CreateRoomViewSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<PackedPrefabData> _injectPrefabData = default;
		private readonly EcsFilterInject<Inc<RoomInfo, RaceInfo>, Exc<RoomViewRef>> _rooms = default;
		private PrefabData _prefabData;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_prefabData = _injectPrefabData.Value.Prefabs;

			foreach (int index in _rooms.Value)
			{
				RoomView roomView = Object.Instantiate(SelectRoom(index));
				roomView.Entity = index;

				_world.AddComponent<RoomViewRef>(index)
					.Value = roomView;

				_world.AddComponent<AddPlayerCommand>(index);
			}
		}

		private RoomView SelectRoom(int room)
		{
			RoomType type = _world.GetComponent<RoomInfo>(room).Type;
			RaceType race = _world.GetComponent<RaceInfo>(room).Type;
			RoomView roomView = type switch
			{
				RoomType.Enemy => _prefabData.EnemyRooms[FastRandom.GetEnemyRoom(_prefabData.EnemyRooms.Length)],
				RoomType.Boss => _prefabData.BossRooms[(int)race - 1],
				RoomType.Start => _prefabData.StartRoom,
				_ => throw new ArgumentException()
			};
			return roomView;
		}
	}
}