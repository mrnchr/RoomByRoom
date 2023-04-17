using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class CreateEnemySystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<EnemyData> _enemyData = default;
		private readonly EcsCustomInject<GameInfo> _gameInfo = default;
		private readonly EcsFilterInject<Inc<NextRoomMessage>> _nextRoomMsgs = Idents.Worlds.MessageWorld;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			EcsWorld message = systems.GetWorld(Idents.Worlds.MessageWorld);

			foreach (int index in _nextRoomMsgs.Value)
			{
				ref NextRoomMessage nextRoomMsg = ref message.GetComponent<NextRoomMessage>(index);
				if (nextRoomMsg.Room.Type == RoomType.Enemy)
					CreateEnemies();
				else if (nextRoomMsg.Room.Type == RoomType.Boss)
					CreateEnemy(UnitType.Boss, nextRoomMsg.Race.Type);
			}
		}

		private void CreateEnemies()
		{
			var numberOfEnemies = 0;
			var enemySize = 0;
			int[] enemyWeights = { 2, 3, 4, 5 };

			while (numberOfEnemies < 11 && enemySize < 21)
			{
				// TODO: change to random
				var enemyType = UnitType.Humanoid; // FastRandom.EnemyType;
				CreateEnemy(enemyType, FastRandom.GetEnemyRace());

				numberOfEnemies++;
				enemySize += enemyWeights[(int)enemyType - 1];
			}
		}

		private void CreateEnemy(UnitType type, RaceType race)
		{
			int enemy = _world.NewEntity();

			_world.AddComponent<UnitInfo>(enemy)
				.Assign(x =>
				{
					x.Type = type;
					return x;
				});

			_world.AddComponent<RaceInfo>(enemy)
				.Assign(x =>
				{
					x.Type = race;
					return x;
				});

			_world.AddComponent<ControllerByAI>(enemy);

			if (type == UnitType.Humanoid)
				_world.AddComponent<Bare>(enemy);

			_world.AddComponent<Health>(enemy)
				.Assign(x =>
				{
					x.MaxPoint = FastRandom.GetUnitHp(_gameInfo.Value.RoomCount, type);
					x.CurrentPoint = x.MaxPoint;
					return x;
				});

			_world.AddComponent<Equipment>(enemy)
				.Assign(x =>
				{
					x.ItemList = new List<int>();
					return x;
				});

			_world.AddComponent<UnitPhysicalProtection>(enemy)
				.Assign(
					x =>
					{
						x.RestoreSpeed = _enemyData.Value.Armor.RestoreSpeed;
						x.CantRestoreTime = _enemyData.Value.Armor.BreakRestoreTime;
						return x;
					});
		}
	}
}