using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
	public class DieSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<GameInfo> _gameInfo = default;
		private readonly EcsCustomInject<PackedPrefabData> _prefabData = default;
		private readonly EcsFilterInject<Inc<Health>> _units = default;

		public void Run(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();

			foreach (int index in _units.Value)
			{
				if (world.GetComponent<Health>(index).CurrentPoint != 0)
					continue;
				Debug.Log($"Entity {index} died");

				// TODO: add player death
				if (Utils.IsUnitOf(world, index, UnitType.Player))
					continue;

				int bonus = world.NewEntity();
				world.AddComponent<Bonus>(bonus)
					.Item = FastRandom.CreateItem(world, _prefabData.Value, _gameInfo.Value);
				world.AddComponent<SpawnCommand>(bonus)
					.Coords = world.GetComponent<UnitViewRef>(index).Value.transform.position;

				foreach (int item in world.GetComponent<Equipment>(index).ItemList)
				{
					Object.Destroy(world.GetComponent<ItemViewRef>(item).Value.gameObject);
					world.DelEntity(item);
				}

				Object.Destroy(world.GetComponent<UnitViewRef>(index).Value.gameObject);
				world.DelEntity(index);
			}
		}
	}
}