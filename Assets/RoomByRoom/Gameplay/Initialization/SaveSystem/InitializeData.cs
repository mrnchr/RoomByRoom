using UnityEngine;

namespace RoomByRoom
{
	[CreateAssetMenu(menuName = "RoomByRoom/Data/InitializeData")]
	public class InitializeData : ScriptableObject
	{
		public GameInfo GameInfo;
		public SavedPlayer Player;
		public SavedRoom Room;
		public SavedInventory Inventory;
	}
}