using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class AfterJumpUnitSystem : IEcsRunSystem
	{
		private EcsFilterInject<Inc<Jumpable, UnitViewRef, CantJump>> _units = default;

		public void Run(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();

			foreach (var index in _units.Value)
			{
				UnitView unitView = world.GetComponent<UnitViewRef>(index).Value;

				if (unitView.Rb.velocity.y > 1)
					world.DelComponent<CantJump>(index);
			}
		}
	}
}