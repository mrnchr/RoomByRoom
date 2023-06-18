using System;

namespace RoomByRoom
{
  [Serializable]
  public class GameSave
  {
    public GameInfo Game;
    public PlayerSave Player;
    public RoomSave Room;
    public InventorySave InventorySave;

    public GameSave()
    {
      Game = new GameInfo();
      InventorySave = new InventorySave();
    }

    public void Copy(GameSave from)
    {
      Game = from.Game;
      Player = from.Player;
      Room = from.Room;
      InventorySave = from.InventorySave;
    }
  }
}