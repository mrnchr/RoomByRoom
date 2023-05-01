using System;
using Leopotam.EcsLite;
using RoomByRoom.Utility;

namespace RoomByRoom.UI.Game
{
	public class TurnWindowService
	{
		private readonly EcsWorld _message;

		public TurnWindowService(EcsWorld message)
		{
			_message = message;
		}

		public void SendSwitchWindowMessage(SwitchWindowMessageType type)
		{
			switch (type)
			{
				case SwitchWindowMessageType.Inventory:
					_message.Add<TurnInventoryMessage>(_message.NewEntity());
					break;
				case SwitchWindowMessageType.Pause:
					_message.Add<TurnPauseMessage>(_message.NewEntity());
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}
	}

	public enum SwitchWindowMessageType
	{
		Inventory = 0,
		Pause = 1
	}
}