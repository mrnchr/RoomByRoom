using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class FillEnemyEquipmentSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<CharacteristicService> _charSvc = default;
		private readonly EcsFilterInject<Inc<Equipped>> _items = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (int index in _items.Value) AddItemToEquipment(index);

			foreach (int index in _world.Filter<Bare>().End())
			{
				_charSvc.Value.Calculate(index);
				_world.GetComponent<UnitPhysicalProtection>(index)
					.Assign(x =>
					{
						x.CurrentPoint = x.MaxPoint;
						return x;
					});
			}
		}

		private void AddItemToEquipment(int index)
		{
			int owner = _world.GetComponent<Owned>(index).Owner;
			if (IsCreatedWhileAgo(owner))
				Utils.AddItemToList(_world.GetComponent<Equipment>(owner).ItemList, index);
		}

		private bool IsCreatedWhileAgo(int owner) => _world.HasComponent<Bare>(owner);
	}
}