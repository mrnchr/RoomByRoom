using System.Collections;
using FluentAssertions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.UnityEditor;
using RoomByRoom.Utility;
using UnityEngine;
using UnityEngine.TestTools;

namespace RoomByRoom.Testing.PlayMode
{
	public class RestorationProtectionTest
	{
		[UnityTest]
		public IEnumerator WhenPlayerAttack_AndHitEnemy_ThenHisProtectionShouldBeRestoredInAWhile()
		{
			var exit = false;

			IEnumerator WaitToStopTest()
			{
				yield return new WaitForSeconds(10);
				exit = true;
				LogAssert.Expect(LogType.Exception, "The waiting time for the test has been exceeded");
			}


			// Arrange
			var engObject = new GameObject("Engine (test)");
			var eng = engObject.AddComponent<RestoreProtectionEngine>();

			yield return new WaitWhile(() => eng.World == null || exit);
			eng.StartCoroutine(WaitToStopTest());

			var roomObj = (GameObject)Object.Instantiate(Resources.Load("StartRoom Variant"));
			var roomView = roomObj.GetComponent<RoomView>();

			int player = eng.World.NewEntity();
			var playerObj = (GameObject)Object.Instantiate(Resources.Load("Player Variant"));
			var playerView = playerObj.GetComponent<PlayerView>();
			playerView.Entity = player;
			playerView.AttackCtr.SetService(eng.AttackSvc);

			int enemy = eng.World.NewEntity();
			var enemyObj = (GameObject)Object.Instantiate(Resources.Load("HumanoidEnemy Variant"));
			var enemyView = enemyObj.GetComponent<HumanoidView>();
			enemyView.Entity = enemy;
			enemyView.AttackCtr.SetService(eng.AttackSvc);

			playerView.transform.position = roomView.SpawnPoints[0].UnitSpawn.position;
			enemyView.transform.position = playerView.transform.position + new Vector3(1f, 0, 1.5f);

			int weapon = eng.World.NewEntity();
			eng.World.AddComponent<WeaponInfo>(weapon).Type = WeaponType.OneHand;
			var weaponObj = (GameObject)Object.Instantiate(Resources.Load("Axe1 Variant"));
			var weaponView = weaponObj.GetComponent<WeaponView>();
			weaponView.Entity = weapon;

			ItemPlace place = playerView.GetWeaponPlace(WeaponType.OneHand);
			weaponView.transform.SetParent(place.Parent);
			weaponView.transform.position = place.Point.position;
			weaponView.transform.rotation = place.Point.rotation;

			// Engine eng = Object.FindObjectOfType<Engine>();
			// Debug.Log(eng);
			// yield return new WaitForSeconds(5);
			EcsWorld world = eng.World;
			world.AddComponent<UnitViewRef>(player).Value = playerView;
			world.AddComponent<UnitViewRef>(enemy).Value = enemyView;
			world.AddComponent<ItemViewRef>(weapon).Value = weaponView;
			world.AddComponent<UnitInfo>(player).Type = UnitType.Player;
			world.AddComponent<UnitInfo>(enemy).Type = UnitType.Humanoid;
			world.AddComponent<MainWeapon>(player).Entity = weapon;
			world.AddComponent<Owned>(weapon).Owner = player;
			world.AddComponent<ItemPhysicalDamage>(weapon).Point = 30;

			UnitPhysicalProtection physProtection = world.AddComponent<UnitPhysicalProtection>(enemy)
				.Assign(x =>
				{
					x.CurrentPoint = 50;
					x.MaxPoint = 100;
					x.RestoreSpeed = 2;
					x.CantRestoreTime = 1;
					return x;
				});

			float lastPoint = physProtection.CurrentPoint;
			world.AddComponent<AttackCommand>(player);

			// Act
			yield return new WaitUntil(() => world.HasComponent<CantRestore>(enemy) || exit);
			var physProt = world.GetComponent<UnitPhysicalProtection>(enemy);
			physProt.CurrentPoint.Should().BeLessThan(lastPoint);
			lastPoint = physProt.CurrentPoint;

			world.DelComponent<CantRestore>(enemy);
			yield return new WaitForSeconds(1);

			// Assert
			world.GetComponent<UnitPhysicalProtection>(enemy).CurrentPoint.Should().BeGreaterThan(lastPoint);
		}
	}

	public class RestoreProtectionEngine : TestEngine
	{
		public override void Start()
		{
			World = new EcsWorld();
			Message = new EcsWorld();
			Systems = new EcsSystems(World);
			AttackSvc = new AttackService(World, Message);
			Systems
				.AddWorld(Message, Idents.Worlds.MessageWorld)
				.Add(new AttackSystem())
				.Add(new TimerSystem<CantRestore>())
				.Add(new DamageSystem())
				.Add(new RestoreProtectionSystem())
				.DelHere<AttackCommand>()
				.Inject(AttackSvc)
				.Add(new EcsWorldDebugSystem())
				.Init();
		}
	}
}