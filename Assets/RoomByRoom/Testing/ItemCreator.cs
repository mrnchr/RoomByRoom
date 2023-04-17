using Leopotam.EcsLite;
using UnityEngine;

namespace RoomByRoom.Testing
{
	public class ItemCreator : ICreator
	{
		public ArmorType Armor = ArmorType.Boots;
		public bool HasEquipped = false;
		public bool HasInHands = false;
		public bool HasOwner = false;
		public bool HasShape = false;
		public bool HasView = false;
		public int Owner = int.MinValue;
		public float PhysicalDamage = 0;
		public float PhysicalProtection = 0;
		public int Shape = int.MinValue;
		public ItemType Type = ItemType.Armor;
		public ItemView View = new GameObject().AddComponent<ItemView>();
		public WeaponType Weapon = WeaponType.Bow;

		public int CreateEntity(EcsWorld world)
		{
			int entity = world.NewEntity();
			Create.ItemInfoCmp(world, entity, Type);
			if (Type == ItemType.Armor)
			{
				Create.ArmorInfoCmp(world, entity, Armor);
				Create.ItemPhysicalProtectionCmp(world, entity, PhysicalProtection);
			}
			else if (Type == ItemType.Weapon)
			{
				Create.WeaponInfoCmp(world, entity, Weapon);
				Create.ItemPhysicalDamageCmp(world, entity, PhysicalDamage);
			}

			if (HasOwner)
				Create.OwnedCmp(world, entity, Owner);

			if (HasShape)
				Create.ShapeCmp(world, entity, Shape);

			if (HasEquipped)
				Create.EquippedCmp(world, entity);

			if (HasInHands && Type == ItemType.Weapon)
				Create.InHandsCmp(world, entity);

			if (HasView)
				Create.ItemViewRefCmp(world, entity, View);

			return entity;
		}
	}
}