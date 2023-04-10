using System;
using UnityEngine.Serialization;

namespace RoomByRoom
{
	[Serializable]
	public struct PlayerEntity
	{
		public RaceInfo Race;
		public Health HealthCmp;
		public Movable MovableCmp;
		public Jumpable JumpableCmp;
		public UnitPhysicalProtection UnitPhysProtectionCmp;
	}
}