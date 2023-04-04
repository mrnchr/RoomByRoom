using System;

namespace RoomByRoom
{
    [Serializable]
    public class SavedData
    {
        public GameInfo GameSave;
        public PlayerEntity Player;
        public RoomEntity Room;
        public InventoryEntity Inventory;

        public SavedData()
        {
            GameSave = new GameInfo();
            Inventory = new InventoryEntity();
        }

        public void CopyOf(SavedData from)
        {
            GameSave = from.GameSave;
            Player = from.Player;
            Room = from.Room;
            Inventory = from.Inventory;
        }
    }
}