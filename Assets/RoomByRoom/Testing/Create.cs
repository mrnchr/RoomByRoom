using Leopotam.EcsLite;

using RoomByRoom;
using RoomByRoom.Utility;

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

	public static ref UnitPhysicalProtection UnitPhysicalProtectionCmp(EcsWorld world, int unit, int currPoint = 0, int maxPoint = 0)
	{
		return ref world.AddComponent<UnitPhysicalProtection>(unit)
			.Assign(x =>
			{
				x.MaxPoint = maxPoint;
				x.CurrentPoint = currPoint;
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

	public static ref ItemPhysicalDamage ItemPhysicalDamageCmp(EcsWorld world, int weapon, float point)
	{
		return ref world.AddComponent<ItemPhysicalDamage>(weapon)
			.Assign(x =>
			{
				x.Point = point;
				return x;
			});
	}
}
