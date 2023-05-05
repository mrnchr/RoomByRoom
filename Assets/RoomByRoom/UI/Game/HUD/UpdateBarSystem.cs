using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.UI.Game.HUD;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class UpdateBarSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<UnitViewRef>> _units = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			
			foreach (int index in _units.Value)
			{
				UnitView unitView = _units.Pools.Inc1.Get(index).Value;
				ref Health health = ref _world.Get<Health>(index);
				ref UnitPhysicalProtection physProtection = ref _world.Get<UnitPhysicalProtection>(index);
				
				SetBarValues(unitView.ArmorBar, physProtection.CurrentPoint, physProtection.MaxPoint);
				SetBarValues(unitView.HealthBar, health.CurrentPoint, health.MaxPoint);
			}
		}

		private static void SetBarValues(Bar bar, float value, float maxValue)
		{
			bar.SetValue(value);
			bar.SetMaxValue(maxValue);
		}
	}
}