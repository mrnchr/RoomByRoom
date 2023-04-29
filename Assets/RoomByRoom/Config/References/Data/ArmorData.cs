using UnityEngine;

namespace RoomByRoom
{
	[CreateAssetMenu(menuName = "RoomByRoom/Data/ArmorData")]
	public class ArmorData : ScriptableObject
	{
		public float RestoreSpeed;
		public float BreakRestoreTime;
	}
}