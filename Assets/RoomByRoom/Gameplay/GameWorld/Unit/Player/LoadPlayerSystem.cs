using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
	internal class LoadPlayerSystem : IEcsInitSystem
	{
		private readonly EcsCustomInject<SavedData> _savedData = default;

		public void Init(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			int player = world.NewEntity();
			PlayerEntity playerEntity = _savedData.Value.Player;

			world.AddComponent<RaceInfo>(player)
				.Assign(x => playerEntity.Race);
			
			// TODO: write test
#if UNITY_EDITOR
			if(playerEntity.HealthCmp.CurrentPoint != playerEntity.HealthCmp.MaxPoint)
				UnityEngine.Debug.LogWarning(
					"Player's current HP doesn't match player's max HP in player saves. The game might work uncorrected");
#endif

			world.AddComponent<Health>(player)
				.Assign(x =>
				{
					x.CurrentPoint = playerEntity.HealthCmp.CurrentPoint;
					return x;
				});

			world.AddComponent<UnitInfo>(player)
				.Assign(x =>
				{
					x.Type = UnitType.Player;
					return x;
				});

			// // TODO: remove after tests
			world.AddComponent<Opener>(player);

			world.AddComponent<ControllerByPlayer>(player);
		}
	}
}