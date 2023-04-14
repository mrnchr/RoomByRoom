using System.Collections;

using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class JumpUnitSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<Jumpable, UnitViewRef, JumpCommand>, Exc<CantJump>> _units = default;
		private readonly EcsCustomInject<CoroutineStarter> _corStarter = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (var index in _units.Value)
			{
				GroundUnitView groundUnit = (GroundUnitView)(_world.GetComponent<UnitViewRef>(index).Value);
				ref Jumpable jumpable = ref _world.GetComponent<Jumpable>(index);

				if (Physics.CheckSphere(groundUnit.transform.position, 0.01f, groundUnit.GroundMask,
					    QueryTriggerInteraction.Ignore))
				{
					groundUnit.Rb.velocity = Vector3.Scale(groundUnit.Rb.velocity, new Vector3(1, 0, 1));
					groundUnit.Rb.AddForce(Vector3.up * jumpable.JumpForce, ForceMode.Impulse);
					_corStarter.Value.PerformCoroutine(WaitToJump(index));
				}
			}
		}

		private IEnumerator WaitToJump(int entity)
		{
			_world.AddComponent<CantJump>(entity);
			yield return new WaitForSeconds(0.5f);
			_world.DelComponent<CantJump>(entity);
		}
	}
}