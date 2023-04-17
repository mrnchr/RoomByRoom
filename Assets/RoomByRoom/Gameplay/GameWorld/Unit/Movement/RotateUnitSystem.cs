using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
	public class RotateUnitSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<RotateCommand, UnitViewRef>> _units = default;

		public void Run(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();

			foreach (int index in _units.Value)
			{
				Vector3 rotateDirection = world.GetComponent<RotateCommand>(index).RotateDirection;

				if (IsMoving(rotateDirection))
				{
					UnitView unitView = world.GetComponent<UnitViewRef>(index).Value;

					if (unitView is PlayerView player)
						RotatePlayer(rotateDirection, player);
					else
						RotateUnit(rotateDirection, unitView);
				}
			}
		}

		private bool IsMoving(Vector3 rotateDirection) => rotateDirection != Vector3.zero;

		private void RotateUnit(Vector3 rotateDirection, UnitView unitView)
		{
			unitView.transform.forward = rotateDirection.normalized;
		}

		private void RotatePlayer(Vector3 rotateDirection, PlayerView player)
		{
			Vector3 forward = player.CameraHolder.TransformDirection(rotateDirection);
			forward.y = 0;
			player.Character.forward = forward.normalized;
		}
	}
}