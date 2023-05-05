using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.UI.Game;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class UpdateInventorySystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<WindowChangedMessage>> _windowMsgs = Idents.Worlds.MessageWorld;
		private readonly EcsCustomInject<BlockingService> _blockingSvc = default;
		private readonly EcsCustomInject<KeepDirtyService> _keepDirtySvc = default;
		private readonly EcsCustomInject<GameMediator> _mediator = default;

		public void Run(IEcsSystems systems)
		{
			if (!_blockingSvc.Value.IsInventory() || _windowMsgs.Value.GetEntitiesCount() <= 0) return;
			_keepDirtySvc.Value.UpdateDirtyMessage(DirtyType.Slots | DirtyType.PlayerModel);
			_mediator.Value.UpdateItemDescription();
			_mediator.Value.UpdateItemRender();
		}
	}
}