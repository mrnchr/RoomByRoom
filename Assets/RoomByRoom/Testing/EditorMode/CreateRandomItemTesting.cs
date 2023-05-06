using FluentAssertions;
using Leopotam.EcsLite;
using NUnit.Framework;
using RoomByRoom.Utility;

namespace RoomByRoom.Testing.EditorMode
{
  public class CreateRandomItemTesting
  {
    [Test]
    public void WhenCreateRandomItem_AndMoreNoting_ThenItemShouldBeHadAllTheComponents()
    {
      // Arrange
      var world = new EcsWorld();
      PrefabService prefabService = Create.PrefabService();

      GameInfo gameInfo = Create.GameInfo();

      for (var i = 0; i < 100; i++)
      {
        // Act
        int item = FastRandom.CreateItem(world, prefabService, gameInfo);

        // Assert
        world.Has<ItemInfo>(item).Should().Be(true);
        world.Has<Shape>(item).Should().Be(true);
        if (world.Get<ItemInfo>(item).Type == ItemType.Armor)
        {
          world.Has<ArmorInfo>(item).Should().Be(true);
          world.Has<ItemPhysicalProtection>(item).Should().Be(true);
        }
        else
        {
          world.Has<WeaponInfo>(item).Should().Be(true);
          world.Has<ItemPhysicalDamage>(item).Should().Be(true);
        }
      }
    }
  }
}