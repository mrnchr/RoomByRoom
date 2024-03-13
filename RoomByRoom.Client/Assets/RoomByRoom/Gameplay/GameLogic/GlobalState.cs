using System;
using RoomByRoom.UI.Game;

namespace RoomByRoom
{
  [Serializable]
  public class GlobalState
  {
    public bool IsWin;
    public WindowType OpenedWindow;
  }
}