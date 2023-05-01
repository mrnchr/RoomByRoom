using System;
using RoomByRoom.UI.Game;

namespace RoomByRoom
{
	[Serializable]
	public class GameInfo
	{
		public int RoomCount;
		public int Money;
		public WindowType OpenedWindow;
	}
}