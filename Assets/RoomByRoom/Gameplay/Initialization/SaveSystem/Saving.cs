using System;

namespace RoomByRoom
{
	[Serializable]
	public class Saving
	{
		public GameInfo GameSave;
		public SavedPlayer Player;
		public SavedRoom Room;
		public SavedInventory Inventory;

		public Saving()
		{
			GameSave = new GameInfo();
			Inventory = new SavedInventory();
		}

		public void CopyOf(Saving from)
		{
			GameSave = from.GameSave;
			Player = from.Player;
			Room = from.Room;
			Inventory = from.Inventory;
		}
	}
}