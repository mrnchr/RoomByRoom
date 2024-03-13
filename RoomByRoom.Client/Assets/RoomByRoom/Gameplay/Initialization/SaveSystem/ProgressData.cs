using System;

namespace RoomByRoom
{
  [Serializable]
  public class ProgressData
  {
    public GameInfo Game = new GameInfo();
    public PlayerSave Player;
    public RoomSave Room;
    public InventorySave InventorySave = new InventorySave();

    public void Copy(ProgressData from)
    {
      Game = new GameInfo { RoomCount = from.Game.RoomCount };
      Player = from.Player;
      Room = from.Room;
      InventorySave = from.InventorySave;
    }
  }
}