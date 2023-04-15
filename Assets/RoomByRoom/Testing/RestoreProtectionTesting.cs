using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Leopotam.EcsLite;
using NUnit.Framework;
using RoomByRoom.Utility;
using UnityEngine;
using UnityEngine.TestTools;

namespace RoomByRoom.Testing
{
	public class RestoreProtectionTesting
	{
		private RestoreProtectionSystem _testSystem;
		private EcsWorld _world;
		private IEcsSystems _systems;
		private int _unit;

		[SetUp]
		public void Setup()
		{
			_testSystem = new RestoreProtectionSystem();
			_world = new EcsWorld();
			_systems = Testing.Setup.Systems(new EcsSystems(_world), _testSystem);

			_unit = _world.NewEntity();
		}

		[Test]
		public void WhenRestoreProtection_AndPhysicalProtectionLessThanMaxPhysicalProtection_ThenPhysicalProtectionShouldBeGreaterThanItWas()
		{
			// Arrange
			ref UnitPhysicalProtection physProtection = ref Create.UnitPhysicalProtectionCmp(_world, _unit, 200, 300, 5);
			float lastPoint = physProtection.CurrentPoint;

			// Act
			for (int i = 0; i < 20; i++)
				_testSystem.Run(_systems);

			// Assert
			physProtection.CurrentPoint.Should().BeGreaterThan(lastPoint);
		}

		[Test]
		public void WhenRestoreProtection_AndPhysicalProtectionCloseByMaxPhysicalProtection_ThenPhysicalProtectionShouldBeMaxPhysicalProtection()
		{
			// Arrange
			ref UnitPhysicalProtection physProtection = ref Create.UnitPhysicalProtectionCmp(_world, _unit, 299, 300, 50);
			float lastPoint = physProtection.CurrentPoint;

			// Act
			for (int i = 0; i < 20; i++)
				_testSystem.Run(_systems);
			
			// Assert
			physProtection.CurrentPoint.Should().Be(physProtection.MaxPoint);
		}

		[TearDown]
		public void TearDown()
		{
			_systems.Destroy();
			_systems = null;
			
			_world.Destroy();
			_world = null;
		}
	}
}