using System;
using UnityEngine.Serialization;

namespace RoomByRoom
{
	[Serializable]
	public struct Health
	{
		public float CurrentPoint;
		[FormerlySerializedAs("Point")] public float MaxPoint;
	}
}