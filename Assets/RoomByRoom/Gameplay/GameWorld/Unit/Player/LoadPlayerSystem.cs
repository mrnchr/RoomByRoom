using System.Collections.Generic;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
	internal class LoadPlayerSystem : IEcsInitSystem
	{
		private readonly EcsCustomInject<Saving> _savedData = default;

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
				.Assign(x => playerEntity.HealthCmp);

			world.AddComponent<UnitInfo>(player)
				.Assign(x =>
				{
					x.Type = UnitType.Player;
					return x;
				});

			world.AddComponent<Inventory>(player)
				.Assign(x => 
				{ 
					x.ItemList = new List<int>(); 
					return x; 
				});
				
			world.AddComponent<Equipment>(player)
				.Assign(x => 
				{ 
					x.ItemList = new List<int>(); 
					return x; 
				});
				
			world.AddComponent<Backpack>(player)
				.Assign(x => 
				{ 
					x.ItemList = new List<int>(); 
					return x; 
				});

			world.AddComponent<UnitPhysicalProtection>(player)
				.Assign(x => playerEntity.UnitPhysProtectionCmp);

			// // TODO: remove after tests
			world.AddComponent<Opener>(player);

			world.AddComponent<ControllerByPlayer>(player);
		}
	}
}