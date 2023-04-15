using Leopotam.EcsLite;
using RoomByRoom.Utility;

namespace RoomByRoom.Testing
{
	public static class Create
	{
		public static ref GetDamageMessage GetDamageMessageCmp(EcsWorld message, int unit, int weapon)
		{
			return ref message.AddComponent<GetDamageMessage>(message.NewEntity())
				.Assign(x =>
				{
					x.Damaged = unit;
					x.Weapon = weapon;
					return x;
				});
		}

		public static ref UnitPhysicalProtection UnitPhysicalProtectionCmp(EcsWorld world,
			int unit,
			float currPoint = 0,
			float maxPoint = 0,
			float restoreSpeed = 0,
			float cantRestoreTime = 0)
		{
			return ref world.AddComponent<UnitPhysicalProtection>(unit)
				.Assign(x =>
				{
					x.MaxPoint = maxPoint;
					x.CurrentPoint = currPoint;
					x.RestoreSpeed = restoreSpeed;
					x.CantRestoreTime = cantRestoreTime; 
					return x;
				});
		}

		public static ref Health HealthCmp(EcsWorld world, int unit, float currPoint = 0, float maxPoint = 0)
		{
			return ref world.AddComponent<Health>(unit)
				.Assign(x =>
				{
					x.CurrentPoint = currPoint;
					x.MaxPoint = maxPoint;
					return x;
				});
		}

		public static ref ItemPhysicalDamage ItemPhysicalDamageCmp(EcsWorld world, int weapon, float point = 0)
		{
			return ref world.AddComponent<ItemPhysicalDamage>(weapon)
				.Assign(x =>
				{
					x.Point = point;
					return x;
				});
		}

		public static ref Owned OwnedCmp(EcsWorld world, int armor, int unit)
		{
			return ref world.AddComponent<Owned>(armor)
				.Assign(x =>
				{
					x.Owner = unit;
					return x;
				});
		}

		public static ref ArmorInfo ArmorInfoCmp(EcsWorld world, int armor, ArmorType type = ArmorType.Boots)
		{
			return ref world.AddComponent<ArmorInfo>(armor)
				.Assign(x =>
				{
					x.Type = type;
					return x;
				});
		}

		public static void EquippedCmp(EcsWorld world, int armor)
		{
			world.AddComponent<Equipped>(armor);
		}

		public static ref ItemPhysicalProtection ItemPhysicalProtection(EcsWorld world, int armor, int point = 0)
		{
			return ref world.AddComponent<ItemPhysicalProtection>(armor)
				.Assign(x =>
				{
					x.Point = point;
					return x;
				});
		}

		public static ref UnitInfo UnitInfoCmp(EcsWorld world, int unit, UnitType type = UnitType.Baby)
		{
			return ref world.AddComponent<UnitInfo>(unit)
				.Assign(x =>
				{
					x.Type = type;
					return x;
				});
		}

		public static ref CantRestore CantRestoreCmp(EcsWorld world, int unit, float timeLeft = 0)
		{
			return ref world.AddComponent<CantRestore>(unit)
				.Assign(x =>
				{
					x.TimeLeft = timeLeft;
					return x;
				});
		}
	}
}