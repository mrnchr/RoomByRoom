using System.Collections.Generic;
using FluentAssertions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using NUnit.Framework;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom.Testing
{
	public class DieEnemyTesting
	{
		[Test]
		public void WhenEnemyDie_AndHeHasZeroHP_ThenHeAndHisEquipmentShouldBeDeleted()
		{
			// Arrange
			DieSystem testSystem = new DieSystem();
			var secondSystem = new WearHumanoidEnemySystem();
			var thirdSystem = new FillEnemyEquipmentSystem();
			var forthSystem = new CreateEquipmentViewSystem();
			EcsWorld world = new EcsWorld();
			IEcsSystems systems = Setup.Systems(new EcsSystems(world), null, testSystem, secondSystem, thirdSystem, forthSystem)
				.Inject(Create.PackedPrefabData(), Create.GameInfo(), new CharacteristicService(world));
			
			int enemy = world.NewEntity();

			world.AddComponent<Bare>(enemy);
			Create.EquipmentCmp(world, enemy);
			Create.HealthCmp(world, enemy);
			Create.UnitPhysicalProtectionCmp(world, enemy);
			Create.UnitInfoCmp(world, enemy);
			Create.UnitViewRefCmp(world, enemy);

			secondSystem.Run(systems);
			thirdSystem.Run(systems);

			foreach (int index in world.GetComponent<Equipment>(enemy).ItemList)
				Create.ItemViewRefCmp(world, index);
			
			// Act
			testSystem.Run(systems);

			// Assert
			world.Filter<ItemViewRef>().Inc<Equipped>().End().GetEntitiesCount().Should().Be(0);
			world.Filter<UnitViewRef>().End().GetEntitiesCount().Should().Be(0);
			
			systems.Destroy();
			systems = null;
			world.Destroy();
			world = null;
		}

		[Test]
		public void WhenEnemyDie_AndHeHasZeroHP_ThenBonusAndItsItemShouldBeCreated()
		{
			// Arrange
			DieSystem testSystem = new DieSystem();
			EcsWorld world = new EcsWorld();
			IEcsSystems systems = Setup.Systems(new EcsSystems(world), null, testSystem)
				.Inject(Create.GameInfo(), Create.PackedPrefabData());

			int enemy = world.NewEntity();
			Create.HealthCmp(world, enemy);
			Create.UnitInfoCmp(world, enemy);
			Create.UnitViewRefCmp(world, enemy);
			Create.EquipmentCmp(world, enemy);

			// Act
			testSystem.Run(systems);
			
			// Assert
			EcsFilter bonuses = world.Filter<Bonus>().End();
			bonuses.GetEntitiesCount().Should().BeGreaterThan(0);
			foreach (int index in bonuses)
			{
				world.HasComponent<SpawnCommand>(index).Should().Be(true);
				int item = world.GetComponent<Bonus>(index).Item;
				world.HasComponent<ItemInfo>(item).Should().Be(true);
			}
			
			systems.Destroy();
			systems = null;
			world.Destroy();
			world = null;
		}
	}
}