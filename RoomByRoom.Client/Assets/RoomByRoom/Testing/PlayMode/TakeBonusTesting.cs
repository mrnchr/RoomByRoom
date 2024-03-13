using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Leopotam.EcsLite;
using NUnit.Framework;
using RoomByRoom.Utility;

namespace RoomByRoom.Testing.PlayMode
{
  public class TakeBonusTesting
  {
    private EcsWorld _world;
    private EcsWorld _message;
    private IEcsSystems _systems;
    private TakeBonusSystem _testSystem;
    private int _bonus;
    private int _hands;
    private int _item;
    private int _player;
    private ItemCreator _itemCreator;
    private List<EcsPackedEntity> _inventory;
    private List<EcsPackedEntity> _equipment;
    private List<EcsPackedEntity> _backpack;

    [SetUp]
    public void SetUp()
    {
      _testSystem = new TakeBonusSystem();
      _world = new EcsWorld();
      _message = new EcsWorld();
      _systems = Setup.Systems(new EcsSystems(_world), _message, _testSystem);

      _player = _world.NewEntity();
      _bonus = _world.NewEntity();
      _world.Add<TakeCommand>(_player);
      Create.UnitViewRefCmp(_world, _player, Create.GO<HumanoidView>());
      _world.Add<Selected>(_bonus);
      _equipment = Create.EquipmentCmp(_world, _player).ItemList;
      _inventory = Create.InventoryCmp(_world, _player).ItemList;
      _backpack = Create.BackpackCmp(_world, _player).ItemList;
      Create.BonusViewRefCmp(_world, _bonus);
      _itemCreator = new ItemCreator
      {
        Type = ItemType.Weapon,
        Weapon = WeaponType.None,
        HasView = true,
        View = null,
        HasOwner = true,
        Owner = _player,
        HasEquipped = true,
        HasInHands = true
      };
      _hands = _itemCreator.CreateEntity(_world);
      _world.Get<Equipment>(_player).ItemList.Add(_world.PackEntity(_hands));
      _world.Get<Inventory>(_player).ItemList.Add(_world.PackEntity(_hands));
      _world.Add<MainWeapon>(_player).Entity = _hands;
    }

    [Test]
    public void WhenTakeBonus_AndEquipmentIsEmpty_ThenItemShouldBeEquipped()
    {
      // Arrange
      _itemCreator = new ItemCreator
      {
        Type = ItemType.Armor
      };
      _item = _itemCreator.CreateEntity(_world);
      _world.Add<Bonus>(_bonus).Item = _item;

      // Act
      _testSystem.Run(_systems);

      // Assert
      _world.Has<Equipped>(_item).Should().Be(true);
      _inventory.Select(_world.Unpack).Should().ContainSingle(x => _item == x);
      _equipment.Select(_world.Unpack).Should().ContainSingle(x => _item == x);
    }

    [Test]
    public void WhenTakeBonus_AndEquipmentHasItemOfTheSameType_ThenItemShouldBeInBackPack()
    {
      // Arrange
      _itemCreator = new ItemCreator
      {
        Type = ItemType.Armor
      };
      int oldItem = _itemCreator.CreateEntity(_world);
      _equipment.Add(_world.PackEntity(oldItem));
      _inventory.Add(_world.PackEntity(oldItem));

      _itemCreator = new ItemCreator
      {
        Type = ItemType.Armor
      };
      _item = _itemCreator.CreateEntity(_world);
      _world.Add<Bonus>(_bonus).Item = _item;

      // Act
      _testSystem.Run(_systems);

      // Assert
      _world.Has<Owned>(_item).Should().Be(true);
      _world.Has<Equipped>(_item).Should().Be(false);
    }

    [Test]
    public void WhenTakeBonus_AndEquipmentHasItemDifferentFromNew_ThenItemShouldBeEquipped()
    {
      // Arrange
      _itemCreator = new ItemCreator
      {
        Type = ItemType.Weapon
      };
      int oldItem = _itemCreator.CreateEntity(_world);
      _equipment.Add(_world.PackEntity(oldItem));
      _inventory.Add(_world.PackEntity(oldItem));

      _itemCreator = new ItemCreator
      {
        Type = ItemType.Armor
      };
      _item = _itemCreator.CreateEntity(_world);
      _world.Add<Bonus>(_bonus).Item = _item;

      // Act
      _testSystem.Run(_systems);

      // Assert
      _world.Has<Owned>(_item).Should().BeTrue();
      _world.Get<Owned>(_item).Owner.Should().Be(_player);
      _world.Has<Equipped>(_item).Should().BeTrue();
    }

    [Test]
    public void WhenTakeBonus_AndEquipmentHasOneHandWeaponAndNewItemIsTwoHandWeapon_ThenNewItemShouldBeInBackpack()
    {
      // Arrange
      _itemCreator = new ItemCreator
      {
        Type = ItemType.Weapon,
        Weapon = WeaponType.OneHand
      };
      int eqItem = _itemCreator.CreateEntity(_world);
      _equipment.Add(_world.PackEntity(eqItem));

      _itemCreator = new ItemCreator
      {
        Type = ItemType.Weapon,
        Weapon = WeaponType.OneHand
      };
      _item = _itemCreator.CreateEntity(_world);
      _world.Add<Bonus>(_bonus).Item = _item;

      // Act
      _testSystem.Run(_systems);

      // Assert
      _backpack.Select(_world.Unpack).Should().ContainSingle(x => x == _item);
    }

    [Test]
    public void WhenTakeBonus_AndEquipmentHasTwoHandWeaponAndNewItemIsOneHandWeapon_ThenNewItemShouldBeInBackpack()
    {
      // Arrange
      _itemCreator = new ItemCreator
      {
        Type = ItemType.Weapon,
        Weapon = WeaponType.TwoHands
      };
      int eqItem = _itemCreator.CreateEntity(_world);
      _equipment.Add(_world.PackEntity(eqItem));

      _itemCreator = new ItemCreator
      {
        Type = ItemType.Weapon,
        Weapon = WeaponType.OneHand
      };
      _item = _itemCreator.CreateEntity(_world);
      _world.Add<Bonus>(_bonus).Item = _item;

      // Act
      _testSystem.Run(_systems);

      // Assert
      _backpack.Select(_world.Unpack).Should().ContainSingle(x => x == _item);
    }

    [Test]
    public void WhenTakeBonus_AndEquipmentHasShieldAndNewItemIsTwoHandWeapon_ThenNewItemShouldBeInBackpack()
    {
      // Arrange
      var creator = new ItemCreator
      {
        Type = ItemType.Armor,
        Armor = ArmorType.Shield
      };
      int eqItem = creator.CreateEntity(_world);
      _equipment.Add(_world.PackEntity(eqItem));

      creator = new ItemCreator
      {
        Type = ItemType.Weapon,
        Weapon = WeaponType.TwoHands
      };
      int newItem = creator.CreateEntity(_world);
      _world.Add<Bonus>(_bonus).Item = newItem;

      // Act
      _testSystem.Run(_systems);

      // Assert
      _backpack.Select(_world.Unpack).Should().ContainSingle(x => x == newItem);
    }

    [Test]
    public void WhenTakeBonus_AndEquipmentHasTwoHandWeaponAndNewItemIsShield_ThenNewItemShouldBeInBackpack()
    {
      // Arrange
      var creator = new ItemCreator
      {
        Type = ItemType.Weapon,
        Weapon = WeaponType.TwoHands
      };
      int eqItem = creator.CreateEntity(_world);
      _equipment.Add(_world.PackEntity(eqItem));

      creator = new ItemCreator
      {
        Type = ItemType.Armor,
        Armor = ArmorType.Shield
      };
      int newItem = creator.CreateEntity(_world);
      _world.Add<Bonus>(_bonus).Item = newItem;

      // Act
      _testSystem.Run(_systems);

      // Assert
      _backpack.Select(_world.Unpack).Should().ContainSingle(x => x == newItem);
    }

    [Test]
    public void WhenTakeBonus_AndEquipmentIsFull_ThenNewItemShouldBeInBackpack()
    {
      // Arrange
      for (int i = _equipment.Count; i < _equipment.Capacity; i++)
        _equipment.Add(_world.PackEntity(i));

      var creator = new ItemCreator
      {
        Type = ItemType.Armor,
        Armor = ArmorType.Shield
      };
      int newItem = creator.CreateEntity(_world);
      _world.Add<Bonus>(_bonus).Item = newItem;

      // Act
      _testSystem.Run(_systems);

      // Assert
      _backpack.Select(_world.Unpack).Should().ContainSingle(x => x == newItem);
    }

    [Test]
    public void WhenTakeBonus_AndInventoryIsFull_ThenBonusAndItemShouldBeNotChanged()
    {
      // Arrange
      for (int i = _equipment.Count; i < _equipment.Capacity; i++)
        _equipment.Add(_world.PackEntity(i));

      for (int i = _backpack.Count; i < _backpack.Capacity; i++)
        _backpack.Add(_world.PackEntity(i));

      var creator = new ItemCreator
      {
        Type = ItemType.Armor,
        Armor = ArmorType.Shield,
        HasView = true
      };
      int newItem = creator.CreateEntity(_world);
      _world.Add<Bonus>(_bonus).Item = newItem;

      // Act
      _testSystem.Run(_systems);

      // Assert
      bool itemViewExists = _world.Has<ItemViewRef>(newItem);
      itemViewExists.Should().BeTrue();
      int numberSelected = _world.Filter<Selected>().End().GetEntitiesCount();
      numberSelected.Should().BeGreaterThan(0);
    }

    [Test]
    public void WhenTakeBonus_AndNewItemIsMeleeWeapon_ThenItemShouldBeInHandsAndMainWeaponOfPlayer()
    {
      // Arrange
      _itemCreator = new ItemCreator
      {
        Type = ItemType.Weapon,
        Weapon = WeaponType.OneHand
      };
      _item = _itemCreator.CreateEntity(_world);
      _world.Add<Bonus>(_bonus).Item = _item;

      // Act
      _testSystem.Run(_systems);

      // Assert
      bool isItemInHands = _world.Has<InHands>(_item);
      isItemInHands.Should().BeTrue();
      var mainWeapon = _world.Get<MainWeapon>(_player);
      mainWeapon.Entity.Should().Be(_item);
    }

    [Test]
    public void WhenTakeBonus_AndNewItemIsMeleeWeapon_ThenHandsShouldBeDisappeared()
    {
      // Arrange
      _itemCreator = new ItemCreator
      {
        Type = ItemType.Weapon,
        Weapon = WeaponType.OneHand
      };
      _item = _itemCreator.CreateEntity(_world);
      Create.BonusCmp(_world, _bonus, _item);

      // Act
      _testSystem.Run(_systems);

      // Assert
      bool areHandsNotVisible = _world.Has<NotVisible>(_hands);
      areHandsNotVisible.Should().BeTrue();
      bool isHandsViewActive = _world.Get<ItemViewRef>(_hands).Value.gameObject.activeSelf;
      isHandsViewActive.Should().BeFalse();
      bool isHandsNotEquipped = _world.Has<Equipped>(_hands);
      isHandsNotEquipped.Should().BeFalse();
      _equipment.Select(_world.Unpack).Should().NotContain(_hands);
      _inventory.Select(_world.Unpack).Should().NotContain(_hands);
    }

    [Test]
    public void WhenTakeBonus_AndNewItemIsMeleeWeaponAndHandsIsMainWeapon_ThenNewItemShouldBeMainWeaponAndInHands()
    {
      // Arrange
      _itemCreator = new ItemCreator
      {
        Type = ItemType.Weapon,
        Weapon = WeaponType.OneHand
      };
      _item = _itemCreator.CreateEntity(_world);
      Create.BonusCmp(_world, _bonus, _item);

      // Act
      _testSystem.Run(_systems);

      // Assert
      bool isHandsInHands = _world.Has<InHands>(_hands);
      isHandsInHands.Should().BeFalse();
      bool isItemInHands = _world.Has<InHands>(_item);
      isItemInHands.Should().BeTrue();
      int mainWeapon = _world.Get<MainWeapon>(_player).Entity;
      mainWeapon.Should().Be(_item);
    }

    [Test]
    public void WhenTakeBonus_AndNewItemIsMeleeWeaponAndHandsIsNotMainWeapon_ThenNewItemShouldBeNotMainWeapon()
    {
      // Arrange
      _itemCreator = new ItemCreator
      {
        Type = ItemType.Weapon,
        Weapon = WeaponType.Bow,
        HasOwner = true,
        Owner = _player,
        HasEquipped = true,
        HasInHands = true
      };
      int bow = _itemCreator.CreateEntity(_world);
      _equipment.Add(_world.PackEntity(bow));
      _inventory.Add(_world.PackEntity(bow));
      _world.Get<MainWeapon>(_player).Entity = bow;

      _world.Del<InHands>(_hands);
      _itemCreator = new ItemCreator
      {
        Type = ItemType.Weapon,
        Weapon = WeaponType.OneHand
      };
      _item = _itemCreator.CreateEntity(_world);
      Create.BonusCmp(_world, _bonus, _item);

      // Act
      _testSystem.Run(_systems);

      // Assert
      bool isItemInHands = _world.Has<InHands>(_item);
      isItemInHands.Should().BeFalse();
      int mainWeapon = _world.Get<MainWeapon>(_player).Entity;
      mainWeapon.Should().Be(bow);
    }
  }
}