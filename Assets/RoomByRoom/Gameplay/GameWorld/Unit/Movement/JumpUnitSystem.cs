using System.Collections;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
	public class JumpUnitSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<CoroutineStarter> _corStarter = default;
		private readonly EcsFilterInject<Inc<Jumpable, UnitViewRef, JumpCommand>, Exc<CantJump>> _units = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (int index in _units.Value)
			{
				var groundUnit = (GroundUnitView)_world.GetComponent<UnitViewRef>(index).Value;
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