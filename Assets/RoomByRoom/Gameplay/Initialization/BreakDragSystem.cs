using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.UI.Game;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class BreakDragSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<WindowChangedMessage>> _changedMsgs = Idents.Worlds.MessageWorld;
		private readonly EcsCustomInject<GameMediator> _mediator = default;
		private readonly EcsCustomInject<BlockingService> _blockingSvc = default;

		public void Run(IEcsSystems systems)
		{
			foreach (int _ in _changedMsgs.Value)
			{
				if (_blockingSvc.Value.CurrentState is WindowType.Pause or WindowType.HUD) 
					_mediator.Value.BreakDragItem();
			}
		}
	}
}