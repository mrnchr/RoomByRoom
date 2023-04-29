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

	[Serializable]
	public struct Health
	{
		public float CurrentPoint;
		[FormerlySerializedAs("Point")] public float MaxPoint;
	}

	public struct CantRestore : ITimerable
	{
		public float TimeLeft { get; set; }
	}
}