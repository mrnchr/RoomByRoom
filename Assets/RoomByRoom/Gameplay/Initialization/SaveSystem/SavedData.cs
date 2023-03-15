using System;

namespace RoomByRoom
{
    [Serializable]
    public class SavedData
    {
        public PlayerEntity Player;
        public RoomEntity Room;
        public InventoryEntity Inventory;

        public void Copy(SavedData from)
        {
            Player = from.Player;
            Room = from.Room;
            Inventory = from.Inventory;
        }
    }
}