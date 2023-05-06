using FluentAssertions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using NUnit.Framework;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom.Testing.EditorMode
{
  public class SelectBonusTesting
  {
    private int _bonus;
    private BonusView _bonusView;
    private int _player;
    private UnitView _playerView;
    private IEcsSystems _systems;
    private SelectBonusSystem _testSystem;
    private EcsWorld _world;

    [SetUp]
    public void SetUp()
    {
      _testSystem = new SelectBonusSystem();
      _world = new EcsWorld();
      _systems = Setup.Systems(new EcsSystems(_world), null, _testSystem)
        .Inject(Create.PlayerData());

      _bonus = _world.NewEntity();
      _player = _world.NewEntity();

      _playerView = Create.UnitViewRefCmp(_world, _player).Value;
      _world.Add<ControllerByPlayer>(_player);
      _bonusView = Create.BonusViewRefCmp(_world, _bonus).Value;
      _world.Add<Bonus>(_bonus);
    }

    [Test]
    public void WhenSelectBonus_AndPlayerIsNearBonus_ThenBonusShouldBeSelected()
    {
      // Arrange

      // Act
      _testSystem.Run(_systems);

      // Assert
      _world.Has<SelectCommand>(_bonus).Should().Be(true);
      _world.Has<Selected>(_bonus).Should().Be(true);
    }

    [Test]
    public void WhenSelectBonus_AndPlayerIsFarFromBonus_ThenBonusShouldBeNotSelected()
    {
      // Arrange
      _playerView.transform.position += new Vector3(20, 20, 20);

      // Act
      _testSystem.Run(_systems);

      // Assert
      _world.Has<SelectCommand>(_bonus).Should().Be(false);
      _world.Has<Selected>(_bonus).Should().Be(false);
    }

    [Test]
    public void
      WhenSelectBonus_AndAnotherBonusWasSelectedAndNewBonusIsNearPlayer_ThenAnotherBonusShouldBeDeselectedAndNewBonusShouldBeSelected()
    {
      // Arrange
      int anotherBonus = _world.NewEntity();
      Create.BonusViewRefCmp(_world, anotherBonus).Value.transform.position = Vector3.forward;
      _world.Add<Bonus>(anotherBonus);
      _world.Add<Selected>(anotherBonus);

      // Act
      _testSystem.Run(_systems);

      // Assert
      _world.Has<Selected>(anotherBonus).Should().Be(false);
      _world.Has<DeselectCommand>(anotherBonus).Should().Be(true);
      _world.Has<SelectCommand>(_bonus).Should().Be(true);
      _world.Has<Selected>(_bonus).Should().Be(true);
    }

    [Test]
    public void
      WhenSelectBonus_AndAnotherBonusWasSelectedAndNewBonusIsFarFromPlayer_ThenAnotherBonusShouldBeDeselectedAndNewBonusShouldBeNotSelected()
    {
      // Arrange
      int anotherBonus = _world.NewEntity();
      Create.BonusViewRefCmp(_world, anotherBonus).Value.transform.position = Vector3.forward * 20;
      _world.Add<Bonus>(anotherBonus);
      _world.Add<Selected>(anotherBonus);
      _bonusView.transform.position = Vector3.back * 20;

      // Act
      _testSystem.Run(_systems);

      // Assert
      _world.Has<Selected>(anotherBonus).Should().Be(false);
      _world.Has<DeselectCommand>(anotherBonus).Should().Be(true);
      _world.Has<SelectCommand>(_bonus).Should().Be(false);
      _world.Has<Selected>(_bonus).Should().Be(false);
    }

    [Test]
    public void WhenSelectBonus_AndBonusWasSelectedAndPlayerIsFarFromBonus_ThenBonusShouldBeDeselected()
    {
      // Arrange
      _bonusView.transform.position = Vector3.back * 20;
      _world.Add<Selected>(_bonus);

      // Act
      _testSystem.Run(_systems);

      // Assert
      _world.Has<Selected>(_bonus).Should().Be(false);
      _world.Has<DeselectCommand>(_bonus).Should().Be(true);
    }

    [Test]
    public void WhenSelectBonus_AndBonusWasSelectedAndPlayerIsNearBonus_ThenBonusShouldBeSelected()
    {
      // Arrange
      _world.Add<Selected>(_bonus);

      // Act
      _testSystem.Run(_systems);

      // Assert
      _world.Has<Selected>(_bonus).Should().Be(true);
      _world.Has<DeselectCommand>(_bonus).Should().Be(false);
      _world.Has<SelectCommand>(_bonus).Should().Be(false);
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