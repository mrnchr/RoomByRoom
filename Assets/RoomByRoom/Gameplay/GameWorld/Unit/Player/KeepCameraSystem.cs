using Leopotam.EcsLite;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
	public class KeepCameraSystem : IEcsRunSystem
	{
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			PlayerView player = GetPlayer();

			player.Camera.position = Physics.Raycast(player.CameraHolder.position,
				GetDirectionFromPlayerToCamera(player), out RaycastHit hit, player.CameraDistance, player.Wall)
				? hit.point - GetDirectionFromPlayerToCamera(player) * 0.05f
				: player.CameraHolder.position + GetDirectionFromPlayerToCamera(player) * player.CameraDistance;
		}

		private Vector3 GetDirectionFromPlayerToCamera(PlayerView player) =>
			(player.Camera.position - player.CameraHolder.position).normalized;

		private PlayerView GetPlayer()
		{
			int entity = _world.Filter<ControllerByPlayer>().End().GetRawEntities()[0];
			return (PlayerView)_world.GetComponent<UnitViewRef>(entity).Value;
		}
	}
}