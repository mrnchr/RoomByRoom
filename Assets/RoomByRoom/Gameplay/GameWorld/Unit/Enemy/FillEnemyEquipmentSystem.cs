using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class FillEnemyEquipmentSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<CharacteristicService> _charSvc = default;
		private readonly EcsFilterInject<Inc<Equipped>> _items = default;
		private readonly EcsFilterInject<Inc<Bare>> _bares = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (int index in _items.Value) 
				AddItemToEquipment(index);

			foreach (int index in _bares.Value)
			{
				_charSvc.Value.Calculate(index);
				_world.Get<UnitPhysicalProtection>(index)
					.Assign(x =>
					{
						x.CurrentPoint = x.MaxPoint;
						return x;
					});
			}
		}

		private void AddItemToEquipment(int item)
		{
			int owner = _world.Get<Owned>(item).Owner;
			if (IsBare(owner))
				Utils.AddItemToList(_world.Get<Equipment>(owner).ItemList, _world.PackEntity(item));
		}

		private bool IsBare(int owner) => _world.Has<Bare>(owner);
	}
}