using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace RoomByRoom
{
	[Serializable]
	public class InventoryEntity
	{
		public List<BoundComponent<ItemInfo>> Item = new();
		public List<BoundComponent<WeaponInfo>> Weapon = new();
		public List<BoundComponent<ArmorInfo>> Armor = new();
		public List<BoundComponent<ItemPhysicalDamage>> PhysDamage = new();
		public List<BoundComponent<ItemPhysicalProtection>> PhysProtection = new();
		public List<BoundComponent<Equipped>> Equipped = new();
		public List<BoundComponent<Shape>> Shape = new();
	}
}