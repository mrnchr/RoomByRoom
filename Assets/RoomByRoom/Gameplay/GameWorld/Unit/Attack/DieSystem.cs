using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
	public class DieSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<GameInfo> _gameInfo = default;
		private readonly EcsCustomInject<PrefabService> _prefabData = default;
		private readonly EcsFilterInject<Inc<Health>> _units = default;

		public void Run(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();

			foreach (int index in _units.Value)
			{
				if (world.Get<Health>(index).CurrentPoint > 0)
					continue;
				Debug.Log($"Entity {index} died");

				// TODO: add player death
				if (Utils.IsUnitOf(world, index, UnitType.Player))
					continue;

				int bonus = world.NewEntity();
				world.Add<Bonus>(bonus)
					.Item = FastRandom.CreateItem(world, _prefabData.Value, _gameInfo.Value);
				world.Add<SpawnCommand>(bonus)
					.Coords = world.Get<UnitViewRef>(index).Value.transform.position;

				foreach (int item in world.Get<Equipment>(index).ItemList)
				{
					Object.Destroy(world.Get<ItemViewRef>(item).Value.gameObject);
					world.DelEntity(item);
				}

				Object.Destroy(world.Get<UnitViewRef>(index).Value.gameObject);
				world.DelEntity(index);
			}
		}
	}
}