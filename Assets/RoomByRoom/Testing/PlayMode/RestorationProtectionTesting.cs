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
        yield return new WaitForSeconds(200);
        exit = true;
        LogAssert.Expect(LogType.Exception, "The waiting time for the test has been exceeded");
      }


      // Arrange
      var engObject = new GameObject("Engine (test)");
      var eng = engObject.AddComponent<RestoreProtectionEngine>();

      yield return new WaitWhile(() => eng.World == null || exit);
      eng.StartCoroutine(WaitToStopTest());

      var camera = new GameObject().AddComponent<Camera>();
      camera.tag = "MainCamera";

      var roomObj = (GameObject)Object.Instantiate(Resources.Load("StartRoom Variant"));
      var roomView = roomObj.GetComponent<RoomView>();

      int player = eng.World.NewEntity();
      var playerObj = (GameObject)Object.Instantiate(Resources.Load("Player Variant"));
      var playerView = playerObj.GetComponent<PlayerView>();
      playerView.Entity = player;
      playerView.AttackCtr.SetService(eng.AttackSvc);

      int enemy = eng.World.NewEntity();
      var enemyObj = (GameObject)Object.Instantiate(Resources.Load("DarkHumanoid Variant"));
      var enemyView = enemyObj.GetComponent<HumanoidView>();
      enemyView.Entity = enemy;
      enemyView.AttackCtr.SetService(eng.AttackSvc);

      Transform plTransform = playerView.transform;
      plTransform.position = roomView.SpawnPoints[0].UnitSpawn.position;
      enemyView.transform.position = plTransform.position + new Vector3(0.5f, 0, 0.5f);

      int weapon = eng.World.NewEntity();
      eng.World.Add<WeaponInfo>(weapon).Type = WeaponType.OneHand;
      var weaponObj = (GameObject)Object.Instantiate(Resources.Load("Axe1 Variant"));
      var weaponView = weaponObj.GetComponent<WeaponView>();
      weaponView.Entity = weapon;

      Transform place = playerView.GetWeaponPlace(WeaponType.OneHand);
      Utils.PutItemInPlace(weaponView.transform, place);

      // Engine eng = Object.FindObjectOfType<Engine>();
      // Debug.Log(eng);
      // yield return new WaitForSeconds(5);
      EcsWorld world = eng.World;
      world.Add<UnitViewRef>(player).Value = playerView;
      world.Add<UnitViewRef>(enemy).Value = enemyView;
      world.Add<ItemViewRef>(weapon).Value = weaponView;
      world.Add<UnitInfo>(player).Type = UnitType.Player;
      world.Add<UnitInfo>(enemy).Type = UnitType.Humanoid;
      world.Add<MainWeapon>(player).Entity = weapon;
      world.Add<Owned>(weapon).Owner = player;
      world.Add<ItemPhysicalDamage>(weapon).Point = 30;

      UnitPhysicalProtection physProtection = world.Add<UnitPhysicalProtection>(enemy)
        .Assign(x =>
        {
          x.CurrentPoint = 50;
          x.MaxPoint = 100;
          x.RestoreSpeed = 2;
          x.CantRestoreTime = 1;
          return x;
        });

      float lastPoint = physProtection.CurrentPoint;
      world.Add<AttackCommand>(player);

      // Act
      yield return new WaitUntil(() => world.Has<CantRestore>(enemy) || exit);
      var physProt = world.Get<UnitPhysicalProtection>(enemy);
      physProt.CurrentPoint.Should().BeLessThan(lastPoint);
      lastPoint = physProt.CurrentPoint;

      world.Del<CantRestore>(enemy);
      yield return new WaitForSeconds(1);

      // Assert
      world.Get<UnitPhysicalProtection>(enemy).CurrentPoint.Should().BeGreaterThan(lastPoint);
    }
  }

  public class RestoreProtectionEngine : TestEngine
  {
    public override void Start()
    {
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