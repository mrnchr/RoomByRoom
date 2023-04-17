using System;
using UnityEngine.Serialization;

namespace RoomByRoom
{
	[Serializable]
	public struct UnitPhysicalProtection
	{
		public float CurrentPoint;
		public float MaxPoint;

		[FormerlySerializedAs("BreakRestoreTime")]
		public float CantRestoreTime;

		public float RestoreSpeed;
	}
}