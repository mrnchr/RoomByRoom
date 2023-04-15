using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
	public class RestoreProtectionSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<UnitPhysicalProtection>, Exc<CantRestore>> _units = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (int index in _units.Value)
			{
				ref UnitPhysicalProtection physProtection = ref _world.GetComponent<UnitPhysicalProtection>(index);
				physProtection.CurrentPoint += physProtection.RestoreSpeed * Time.deltaTime;
				physProtection.CurrentPoint.Clamp(max: physProtection.MaxPoint);
			}
		}
	}
}