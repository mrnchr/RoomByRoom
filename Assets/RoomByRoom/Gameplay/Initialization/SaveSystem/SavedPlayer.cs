using System;

namespace RoomByRoom
{
	[Serializable]
	public struct SavedPlayer
	{
		public RaceInfo Race;
		public Health HealthCmp;
		public Movable MovableCmp;
		public Jumpable JumpableCmp;
		public UnitPhysicalProtection UnitPhysProtectionCmp;
	}
}