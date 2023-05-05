using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.UI.Game;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class WindowStateSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<TurnInventoryMessage>> _invMsgs = Idents.Worlds.MessageWorld;
		private readonly EcsFilterInject<Inc<TurnPauseMessage>> _pauseMsgs = Idents.Worlds.MessageWorld;
		private readonly EcsFilterInject<Inc<PlayerDyingMessage>> _dieMsgs = Idents.Worlds.MessageWorld;
		private readonly EcsFilterInject<Inc<WinMessage>> _winMsgs = Idents.Worlds.MessageWorld;
		private readonly EcsCustomInject<GameInfo> _gameInfo = default;
		private readonly EcsCustomInject<GameMediator> _mediator = default;
		private EcsWorld _message;

		public void Run(IEcsSystems systems)
		{
			bool turnInv = _invMsgs.Value.GetEntitiesCount() > 0;
			bool turnPause = _pauseMsgs.Value.GetEntitiesCount() > 0;
			bool turnLose = _dieMsgs.Value.GetEntitiesCount() > 0;
			bool turnWin = _winMsgs.Value.GetEntitiesCount() > 0;

			if (!turnInv && !turnPause && !turnLose && !turnWin) return;
			// Debug.Log($"{turnInv}, {turnPause}, {turnLose}");
			_message = systems.GetWorld(Idents.Worlds.MessageWorld);

			ChangeWindow(turnLose, turnPause, turnInv, turnWin);

			DelMsgs(_invMsgs);
			DelMsgs(_pauseMsgs);
		}

		private void ChangeWindow(bool turnLose, bool turnPause, bool turnInv, bool turnWin)
		{
			WindowType currWindow = _gameInfo.Value.OpenedWindow;
			WindowType resultWindow = currWindow;
			if (currWindow is WindowType.Lose or WindowType.Win) return;
			if (turnLose)
				resultWindow = WindowType.Lose;
			else if (turnWin)
				resultWindow = WindowType.Win;
			else if (turnPause)
				resultWindow = currWindow == WindowType.Pause ? WindowType.HUD : WindowType.Pause;
			else if (turnInv)
				resultWindow = currWindow == WindowType.Inventory ? WindowType.HUD : WindowType.Inventory;
			
			_gameInfo.Value.OpenedWindow = resultWindow;
			_mediator.Value.TurnWindow(resultWindow);
			_message.Add<WindowChangedMessage>(_message.NewEntity());

			// WindowType resultWindow = currWindow switch
			// {
			// 	WindowType.HUD       when turnLose  => WindowType.Lose,
			// 	WindowType.HUD       when turnPause => WindowType.Pause,
			// 	WindowType.HUD       when turnInv   => WindowType.Inventory,
			// 	WindowType.Inventory when turnLose  => WindowType.Lose,
			// 	WindowType.Inventory when turnPause => WindowType.Pause,
			// 	WindowType.Inventory when turnInv   => WindowType.HUD,
			// 	WindowType.Pause     when turnLose  => WindowType.Lose,
			// 	WindowType.Pause     when turnPause => WindowType.HUD,
			// 	_ => currWindow
			// };

		}

		private void DelMsgs<T>(EcsFilterInject<Inc<T>> filter)
		where T : struct
		{
			foreach (int index in filter.Value)
				filter.Pools.Inc1.Del(index);
		}
	}
}