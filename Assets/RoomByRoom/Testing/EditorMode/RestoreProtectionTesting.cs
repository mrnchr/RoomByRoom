using System.Collections;
using FluentAssertions;
using Leopotam.EcsLite;
using NUnit.Framework;
using RoomByRoom.Utility;
using UnityEngine.TestTools;

namespace RoomByRoom.Testing.EditorMode
{
	public class RestoreProtectionTesting
	{
		private IEcsSystems _systems;
		private RestoreProtectionSystem _testSystem;
		private int _unit;
		private EcsWorld _world;

		[SetUp]
		public void SetUp()
		{
			_testSystem = new RestoreProtectionSystem();
			_world = new EcsWorld();
			_systems = Setup.Systems(new EcsSystems(_world), null, _testSystem);

			_unit = _world.NewEntity();
		}

		[UnityTest]
		public IEnumerator
			WhenRestoreProtection_AndPhysicalProtectionLessThanMaxPhysicalProtection_ThenPhysicalProtectionShouldBeGreaterThanItWas()
		{
			// Arrange
			float lastPoint = Create.UnitPhysicalProtectionCmp(_world, _unit, 200, 300, 5).CurrentPoint;

			// Act
			yield return null;
			_testSystem.Run(_systems);

			// Assert
			_world.Get<UnitPhysicalProtection>(_unit).CurrentPoint.Should().BeGreaterThan(lastPoint);
		}

		[UnityTest]
		public IEnumerator
			WhenRestoreProtection_AndPhysicalProtectionCloseByMaxPhysicalProtection_ThenPhysicalProtectionShouldBeMaxPhysicalProtection()
		{
			// Arrange
			float maxPoint = Create.UnitPhysicalProtectionCmp(_world, _unit, 299, 300, 50).MaxPoint;

			// Act
			for (var i = 0; i < 5; i++)
			{
				yield return null;
				_testSystem.Run(_systems);
			}

			// Assert
			_world.Get<UnitPhysicalProtection>(_unit).CurrentPoint.Should().Be(maxPoint);
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