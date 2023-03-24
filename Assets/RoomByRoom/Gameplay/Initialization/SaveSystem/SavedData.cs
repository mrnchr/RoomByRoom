using System;

namespace RoomByRoom
{
    [Serializable]
    public class SavedData
    {
        public GameInfo GameInfo;
        public PlayerEntity Player;
        public RoomEntity Room;
        public InventoryEntity Inventory;

        public SavedData()
        {
            Player.Ground = new GroundUnitEntity();
            Inventory = new InventoryEntity();
        }

        public void Copy(SavedData from)
        {
            GameInfo = from.GameInfo;
            Player = from.Player;
            Room = from.Room;
            Inventory = from.Inventory;
        }
    }
}