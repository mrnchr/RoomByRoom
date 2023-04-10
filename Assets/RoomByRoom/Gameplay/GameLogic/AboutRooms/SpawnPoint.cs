using System;
using UnityEngine;

namespace RoomByRoom
{
	[Serializable]
	public struct SpawnPoint
	{
		public UnitType UnitType;
		public Transform UnitSpawn;
	}
}