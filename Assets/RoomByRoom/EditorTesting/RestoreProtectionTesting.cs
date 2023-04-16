using System.Collections;
using FluentAssertions;
using Leopotam.EcsLite;
using NUnit.Framework;
using RoomByRoom.Utility;
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
			_systems = Testing.Setup.Systems(new EcsSystems(_world), null, _testSystem);

			_unit = _world.NewEntity();
		}

		[UnityTest]
		public IEnumerator WhenRestoreProtection_AndPhysicalProtectionLessThanMaxPhysicalProtection_ThenPhysicalProtectionShouldBeGreaterThanItWas()
		{
			// Arrange
			float lastPoint = Create.UnitPhysicalProtectionCmp(_world, _unit, 200, 300, 5).CurrentPoint;

			// Act
			yield return null;
			_testSystem.Run(_systems);

			// Assert
			_world.GetComponent<UnitPhysicalProtection>(_unit).CurrentPoint.Should().BeGreaterThan(lastPoint);
		}

		[UnityTest]
		public IEnumerator WhenRestoreProtection_AndPhysicalProtectionCloseByMaxPhysicalProtection_ThenPhysicalProtectionShouldBeMaxPhysicalProtection()
		{
			// Arrange
			float maxPoint = Create.UnitPhysicalProtectionCmp(_world, _unit, 299, 300, 50).MaxPoint;

			// Act
			for (int i = 0; i < 5; i++)
			{
				yield return null;
				_testSystem.Run(_systems);
			}
			
			// Assert
			_world.GetComponent<UnitPhysicalProtection>(_unit).CurrentPoint.Should().Be(maxPoint);
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