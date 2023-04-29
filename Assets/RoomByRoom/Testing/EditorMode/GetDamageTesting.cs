using FluentAssertions;
using Leopotam.EcsLite;
using NUnit.Framework;
using RoomByRoom.Utility;

namespace RoomByRoom.Testing.EditorMode
{
	public class GetDamageTesting
	{
		[Test]
		public void WhenGetDamage_AndProtectionMoreThanOrEqualToDamage_ThenHealthShouldBeNotChanged()
		{
			// Arrange
			var damageSystem = new DamageSystem();
			var world = new EcsWorld();
			var message = new EcsWorld();
			IEcsSystems systems = new EcsSystems(world);
			Setup.Systems(systems, message, damageSystem);
			int unit = world.NewEntity();
			int weapon = world.NewEntity();
			Create.GetDamageMessageCmp(message, message.NewEntity(), unit, weapon);
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
			var damageSystem = new DamageSystem();
			var world = new EcsWorld();
			var message = new EcsWorld();
			IEcsSystems systems = Setup.Systems(new EcsSystems(world), message, damageSystem);

			int unit = world.NewEntity();
			int weapon = world.NewEntity();

			Create.GetDamageMessageCmp(message, message.NewEntity(), unit, weapon);
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
			var damageSystem = new DamageSystem();
			var world = new EcsWorld();
			var message = new EcsWorld();
			IEcsSystems systems = Setup.Systems(new EcsSystems(world), message, damageSystem);

			int unit = world.NewEntity();
			int weapon = world.NewEntity();
			Create.GetDamageMessageCmp(message, message.NewEntity(), unit, weapon);
			ref UnitPhysicalProtection physProtection = ref Create.UnitPhysicalProtectionCmp(world, unit, 90, 90);
			Create.ItemPhysicalDamageCmp(world, weapon, 110);
			Create.HealthCmp(world, unit);

			// Act
			damageSystem.Run(systems);

			// Assert
			physProtection.CurrentPoint.Should().Be(0);
		}

		[Test]
		public void
			WhenGetDamage_AndProtectionLessThanDamageAndHealthMoreThanOrEqualToDifferenceDamageAndProtection_ThenHealthShouldBeLessByDifferenceDamageAndProtection()
		{
			// Arrange
			var damageSystem = new DamageSystem();
			var world = new EcsWorld();
			var message = new EcsWorld();
			IEcsSystems systems = Setup.Systems(new EcsSystems(world), message, damageSystem);

			int unit = world.NewEntity();
			int weapon = world.NewEntity();
			Create.GetDamageMessageCmp(message, message.NewEntity(), unit, weapon);
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
		public void
			WhenGetDamage_AndProtectionLessThanDamageAndHealthLessThanDifferenceDamageAndProtection_ThenHealthShouldBeZero()
		{
			// Arrange
			var damageSystem = new DamageSystem();
			var world = new EcsWorld();
			var message = new EcsWorld();
			IEcsSystems systems = Setup.Systems(new EcsSystems(world), message, damageSystem);

			int unit = world.NewEntity();
			int weapon = world.NewEntity();
			Create.GetDamageMessageCmp(message, message.NewEntity(), unit, weapon);
			Create.UnitPhysicalProtectionCmp(world, unit, 90, 90);
			Create.ItemPhysicalDamageCmp(world, weapon, 110);
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
			var testSystem = new DamageSystem();
			var world = new EcsWorld();
			var message = new EcsWorld();
			IEcsSystems systems = Setup.Systems(new EcsSystems(world), message, testSystem);

			int weapon = world.NewEntity();
			int unit = world.NewEntity();
			int msg = message.NewEntity();
			if (msg == unit)
				msg = message.NewEntity();
			Create.GetDamageMessageCmp(message, msg, unit, weapon);
			float maxTime = Create.UnitPhysicalProtectionCmp(world, unit, cantRestoreTime: 10).CantRestoreTime;
			Create.ItemPhysicalDamageCmp(world, weapon);
			Create.HealthCmp(world, unit);

			// Act
			testSystem.Run(systems);

			// Assert
			world.Has<CantRestore>(unit).Should().Be(true);
			world.Get<CantRestore>(unit).TimeLeft.Should().Be(maxTime);
		}

		[Test]
		public void WhenGetDamage_AndUnitHasCantRestoreCmp_ThenTimeLeftShouldBeMaxCantRestoreTime()
		{
			// Arrange
			const float checkValue = 10;
			const float currValue = 5;
			var testSystem = new DamageSystem();
			var world = new EcsWorld();
			var message = new EcsWorld();
			IEcsSystems systems = Setup.Systems(new EcsSystems(world), message, testSystem);

			int weapon = world.NewEntity();
			int unit = world.NewEntity();
			int msg = message.NewEntity();
			if (msg == unit)
				msg = message.NewEntity();

			Create.GetDamageMessageCmp(message, msg, unit, weapon);
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