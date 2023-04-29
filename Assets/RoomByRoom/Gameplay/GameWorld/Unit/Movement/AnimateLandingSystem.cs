using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
	public class AnimateLandingSystem : IEcsRunSystem
	{
		private EcsFilterInject<Inc<UnitViewRef>> _units = default;

		public void Run(IEcsSystems systems)
		{
			foreach (int index in _units.Value)
			{
				GroundUnitView groundView = (GroundUnitView)_units.Pools.Inc1.Get(index).Value;
				bool checkSphere = Physics.CheckSphere(groundView.transform.position, 0.1f, groundView.GroundMask,
					QueryTriggerInteraction.Ignore);
				if(checkSphere) Debug.Log("Jump");
				if(groundView.Rb.velocity.y < 0 && checkSphere)
					groundView.AnimateJump(false);
			}
		}
	}
}