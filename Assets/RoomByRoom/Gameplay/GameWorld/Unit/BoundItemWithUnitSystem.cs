using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;
using TMPro;
using UnityEngine.UI;

namespace RoomByRoom
{
	public class BoundItemWithUnitSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<ItemPhysicalProtection, Equipped>> _armors = default;
		private readonly EcsFilterInject<Inc<UnitPhysicalProtection>> _units = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (int index in _armors.Value)
			{
				if (_world.GetComponent<ArmorInfo>(index).Type == ArmorType.Shield)
					return;
				
				int owner = _world.GetComponent<Owned>(index).Owner;
				float point = _world.GetComponent<ItemPhysicalDamage>(index).Point;
				_world.GetComponent<UnitPhysicalProtection>(owner)
					.Assign(x =>
					{
						x.MaxPoint += point;
						return x;
					});
			}

			foreach (int index in _units.Value)
			{
				_world.GetComponent<UnitPhysicalProtection>(index)
					.Assign(x =>
					{
						x.CurrentPoint = x.MaxPoint;
						return x;
					});
			}
		}
	}
}