using System.Collections;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
	public class JumpUnitSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<Jumpable, UnitViewRef, JumpCommand>, Exc<CantJump>> _units = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (int index in _units.Value)
			{
				var groundView = (GroundUnitView)_world.Get<UnitViewRef>(index).Value;

				bool checkSphere = Physics.CheckSphere(groundView.transform.position, 0.01f, groundView.GroundMask,
					QueryTriggerInteraction.Ignore);
				
				if (!checkSphere) 
					continue;
				float jumpForce = _world.Get<Jumpable>(index).JumpForce;	
				groundView.Rb.velocity = Vector3.Scale(groundView.Rb.velocity, new Vector3(1, 0, 1));
				groundView.Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
				groundView.AnimateJump(true);
				_world.Add<CantJump>(index)
					.TimeLeft = 0.2f;
			}
		}
	}
}