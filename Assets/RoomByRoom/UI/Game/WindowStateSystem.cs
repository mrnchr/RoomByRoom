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
		private readonly EcsCustomInject<GameInfo> _gameInfo = default;
		private readonly EcsCustomInject<GameMediator> _mediator = default;

		public void Run(IEcsSystems systems)
		{
			bool turnInv = _invMsgs.Value.GetEntitiesCount() > 0;
			bool turnPause = _pauseMsgs.Value.GetEntitiesCount() > 0;

			if (!turnInv && !turnPause) return;
			
			WindowType currWindow = _gameInfo.Value.OpenedWindow;
			WindowType resultWindow = currWindow switch
			{
				WindowType.HUD       when turnInv   => WindowType.Inventory,
				WindowType.HUD       when turnPause => WindowType.Pause,
				WindowType.Inventory when turnInv   => WindowType.HUD,
				WindowType.Inventory when turnPause => WindowType.Pause,
				WindowType.Pause     when turnPause => WindowType.HUD,
				_ => currWindow
			};

			_gameInfo.Value.OpenedWindow = resultWindow;
			_mediator.Value.TurnWindow(resultWindow);

			DelMsgs(_invMsgs);
			DelMsgs(_pauseMsgs);
		}

		private void DelMsgs<T>(EcsFilterInject<Inc<T>> filter)
		where T : struct
		{
			foreach (int index in filter.Value)
				filter.Pools.Inc1.Del(index);
		}
	}
}