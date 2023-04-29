using System;

namespace RoomByRoom
{
	[Serializable]
	public struct WeaponInfo
	{
		public WeaponType Type;
	}

	public struct NotVisible
	{
	}

	public struct InHands
	{
	}

	[Serializable]
	public struct ItemPhysicalDamage
	{
		public float Point;
	}
}