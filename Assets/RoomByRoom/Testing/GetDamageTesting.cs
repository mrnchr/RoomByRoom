using NUnit.Framework;
using FluentAssertions;

using Leopotam.EcsLite;
using RoomByRoom.Utility;

namespace RoomByRoom.Testing
{
	public class GetDamageTesting
	{
		[Test]
		public void WhenGetDamage_AndProtectionMoreThanOrEqualToDamage_ThenHealthShouldBeNotChanged()
		{
			// Arrange
			DamageSystem damageSystem = new DamageSystem();
			EcsWorld world = new EcsWorld();
			EcsWorld message = new EcsWorld();
			IEcsSystems systems = new EcsSystems(world);
			Setup.Systems(systems, damageSystem, message);
			int unit = world.NewEntity();
			int weapon = world.NewEntity();
			Create.GetDamageMessageCmp(message, unit, weapon);
			UnitPhysicalProtection physProtection = Create.UnitPhysicalProtectionCmp(world, unit, 90, 90);

			ref Health health = ref Create.HealthCmp(world, unit, 90, 90);

			float lastCurrentHp = health.CurrentPoint;

			Create.ItemPhysicalDamageCmp(world, weapon, physProtection.CurrentPoint - 1);

			// Act
			damageSystem.Run(systems);

			// Assert
			health.CurrentPoint.Should().Be(lastCurrentHp);
		}

		[Test]
		public void WhenGetDamage_AndProtectionMoreThanOrEqualToDamage_ThenProtectionShouldBeLessByDamage()
		{
			// Arrange
			DamageSystem damageSystem = new DamageSystem();
			EcsWorld world = new EcsWorld();
			EcsWorld message = new EcsWorld();
			IEcsSystems systems = Setup.Systems(new EcsSystems(world), damageSystem, message);
		
			int unit = world.NewEntity();
			int weapon = world.NewEntity();
		
			Create.GetDamageMessageCmp(message, unit, weapon);
			Create.HealthCmp(world, unit, 90, 90);
		
			ref UnitPhysicalProtection physProtection = ref Create.UnitPhysicalProtectionCmp(world, unit, 90, 90);
			float lastCurrentPoint = physProtection.CurrentPoint;
			float damagePoint = Create.ItemPhysicalDamageCmp(world, weapon, 40).Point;

			// Act
			damageSystem.Run(systems);
		
			// Assert
			physProtection.CurrentPoint.Should().Be(lastCurrentPoint - damagePoint);
		}

		[Test]
		public void WhenGetDamage_AndProtectionLessThanDamage_ThenProtectionShouldBeZero()
		{
			// Arrange
			DamageSystem damageSystem = new DamageSystem();
			EcsWorld world = new EcsWorld();
			EcsWorld message = new EcsWorld();
			IEcsSystems systems = Setup.Systems(new EcsSystems(world), damageSystem, message);

			int unit = world.NewEntity();
			int weapon = world.NewEntity();
			Create.GetDamageMessageCmp(message, unit, weapon);
			ref UnitPhysicalProtection physProtection = ref Create.UnitPhysicalProtectionCmp(world, unit, 90, 90);
			Create.ItemPhysicalDamageCmp(world, weapon, 110);
			Create.HealthCmp(world, unit);

			// Act
			damageSystem.Run(systems);
		
			// Assert
			physProtection.CurrentPoint.Should().Be(0);
		}

		[Test]
		public void WhenGetDamage_AndProtectionLessThanDamageAndHealthMoreThanOrEqualToDifferenceDamageAndProtection_ThenHealthShouldBeLessByDifferenceDamageAndProtection()
		{
			// Arrange
			DamageSystem damageSystem = new DamageSystem();
			EcsWorld world = new EcsWorld();
			EcsWorld message = new EcsWorld();
			IEcsSystems systems = Setup.Systems(new EcsSystems(world), damageSystem, message);

			int unit = world.NewEntity();
			int weapon = world.NewEntity();
			Create.GetDamageMessageCmp(message, unit, weapon);
			float physProtection = Create.UnitPhysicalProtectionCmp(world, unit, 90, 90).CurrentPoint;
			float physDamage = Create.ItemPhysicalDamageCmp(world, weapon, 110).Point;
			ref Health health = ref Create.HealthCmp(world, unit, 120, 90);
			float lastCurrentPoint = health.CurrentPoint;
		
			// Act
			damageSystem.Run(systems);

			// Assert
			health.CurrentPoint.Should().Be(lastCurrentPoint - (physDamage - physProtection));
		}

		[Test]
		public void WhenGetDamage_AndProtectionLessThanDamageAndHealthLessThanDifferenceDamageAndProtection_ThenHealthShouldBeZero()
		{
			// Arrange
			DamageSystem damageSystem = new DamageSystem();
			EcsWorld world = new EcsWorld();
			EcsWorld message = new EcsWorld();
			IEcsSystems systems = Setup.Systems(new EcsSystems(world), damageSystem, message);

			int unit = world.NewEntity();
			int weapon = world.NewEntity();
			Create.GetDamageMessageCmp(message, unit, weapon);
			float physProtection = Create.UnitPhysicalProtectionCmp(world, unit, 90, 90).CurrentPoint;
			float physDamage = Create.ItemPhysicalDamageCmp(world, weapon, 110).Point;
			ref Health health = ref Create.HealthCmp(world, unit, 5, 90);
		
			// Act
			damageSystem.Run(systems);
		
			// Assert
			health.CurrentPoint.Should().Be(0);
		}

		[Test]
		public void WhenGetDamage_AndProtectionRestores_ThenLeftTimeShouldBeMaxCantRestoreTime()
		{
			// Arrange
			DamageSystem testSystem = new DamageSystem();
			EcsWorld world = new EcsWorld();
			EcsWorld message = new EcsWorld();
			IEcsSystems systems = Setup.Systems(new EcsSystems(world), testSystem, message);

			int unit = world.NewEntity();
			int weapon = world.NewEntity();
			Create.GetDamageMessageCmp(message, unit, weapon);
			float maxTime = Create.UnitPhysicalProtectionCmp(world, unit).CantRestoreTime;
			Create.ItemPhysicalDamageCmp(world, weapon);
			Create.HealthCmp(world, unit);
			
			// Act
			testSystem.Run(systems);
			
			// Assert
			world.HasComponent<CantRestore>(unit).Should().Be(true);
			world.GetComponent<CantRestore>(unit).TimeLeft.Should().Be(maxTime);
		}

		[Test]
		public void WhenGetDamage_AndUnitHasCantRestoreCmp_ThenTimeLeftShouldBeMaxCantRestoreTime()
		{
			// Arrange
			const float checkValue = 10;
			const float currValue = 5;
			DamageSystem testSystem = new DamageSystem();
			EcsWorld world = new EcsWorld();
			EcsWorld message = new EcsWorld();
			IEcsSystems systems = Setup.Systems(new EcsSystems(world), testSystem, message);

			int unit = world.NewEntity();
			int weapon = world.NewEntity();
			Create.GetDamageMessageCmp(message, unit, weapon);
			Create.UnitPhysicalProtectionCmp(world, unit, cantRestoreTime: checkValue);
			Create.ItemPhysicalDamageCmp(world, weapon);
			Create.HealthCmp(world, unit);
			ref CantRestore cantRestore = ref Create.CantRestoreCmp(world, unit, currValue);

			// Act
			testSystem.Run(systems);
			
			// Assert
			cantRestore.TimeLeft.Should().Be(checkValue);
		}
	}
}