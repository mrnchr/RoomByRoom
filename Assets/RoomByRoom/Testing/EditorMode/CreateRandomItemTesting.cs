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
			PackedPrefabData prefabData = Create.PackedPrefabData();

			GameInfo gameInfo = Create.GameInfo();

			for (var i = 0; i < 100; i++)
			{
				// Act
				int item = FastRandom.CreateItem(world, prefabData, gameInfo);

				// Assert
				world.HasComponent<ItemInfo>(item).Should().Be(true);
				world.HasComponent<Shape>(item).Should().Be(true);
				if (world.GetComponent<ItemInfo>(item).Type == ItemType.Armor)
				{
					world.HasComponent<ArmorInfo>(item).Should().Be(true);
					world.HasComponent<ItemPhysicalProtection>(item).Should().Be(true);
				}
				else
				{
					world.HasComponent<WeaponInfo>(item).Should().Be(true);
					world.HasComponent<ItemPhysicalDamage>(item).Should().Be(true);
				}
			}
		}
	}
}