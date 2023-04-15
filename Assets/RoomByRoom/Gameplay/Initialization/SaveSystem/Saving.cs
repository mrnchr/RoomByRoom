using System;

namespace RoomByRoom
{
	[Serializable]
	public class Saving
	{
		public GameInfo GameSave;
		public PlayerEntity Player;
		public RoomEntity Room;
		public InventoryEntity Inventory;

		public Saving()
		{
			GameSave = new GameInfo();
			Inventory = new InventoryEntity();
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