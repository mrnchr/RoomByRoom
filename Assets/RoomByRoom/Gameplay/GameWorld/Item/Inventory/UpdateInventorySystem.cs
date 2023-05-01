using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.UI.Game;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class UpdateInventorySystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<DirtyMessage>> _dirtyMsgs = Idents.Worlds.MessageWorld;
		private readonly EcsCustomInject<GameMediator> _mediator = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			EcsWorld message = systems.GetWorld(Idents.Worlds.MessageWorld);

			foreach (int index in _dirtyMsgs.Value)
			{
				_mediator.Value.UpdateInventory(GetInventory());
				message.DelEntity(index);
			}
		}

		private List<int> GetInventory()
		{
			int player = _world.Filter<ControllerByPlayer>().End().GetRawEntities()[0];
			return _world.Get<Inventory>(player).ItemList;
		}
	}
}