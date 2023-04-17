using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
	internal class CreatePlayerViewSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<AttackService> _attackSvc = default;
		private readonly EcsCustomInject<PackedPrefabData> _packedPrefabData = default;
		private readonly EcsFilterInject<Inc<ControllerByPlayer>, Exc<UnitViewRef>> _player = default;
		private readonly EcsCustomInject<Saving> _savedData = default;

		public void Run(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();

			foreach (int index in _player.Value)
			{
				// Create player entity from save
				ref PlayerEntity playerEntity = ref _savedData.Value.Player;
				// Spawn player in the world
				GameObject player = Object.Instantiate(_packedPrefabData.Value.Prefabs.BasePlayerUnit.gameObject);
				var playerView = player.GetComponent<PlayerView>();
				playerView.Entity = index;
				playerView.AttackCtr.SetService(_attackSvc.Value);

				// Add PlayerViewRef component
				ref UnitViewRef playerRef = ref world.GetPool<UnitViewRef>().Add(index);
				playerRef.Value = playerView;

				// Add Moving component
				ref Movable movable = ref world.GetPool<Movable>().Add(index);
				movable = playerEntity.MovableCmp;

				// Add Jumping component
				ref Jumpable jumpable = ref world.GetPool<Jumpable>().Add(index);
				jumpable = playerEntity.JumpableCmp;
			}
		}
	}
}