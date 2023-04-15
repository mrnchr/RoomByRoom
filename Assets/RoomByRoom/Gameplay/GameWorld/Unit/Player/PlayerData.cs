using UnityEngine;

namespace RoomByRoom
{
	[CreateAssetMenu(menuName = "RoomByRoom/Data/PlayerData")]
	public class PlayerData : ScriptableObject
	{
		public ArmorData Armor;
	}
}