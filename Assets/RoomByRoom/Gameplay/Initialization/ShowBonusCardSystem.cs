
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.UI.Game;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class ShowBonusCardSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<SelectCommand>> _selectCmds = default;
		private readonly EcsFilterInject<Inc<Selected>> _selected = default;
		private readonly EcsCustomInject<GameMediator> _mediator = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			if (_selectCmds.Value.GetEntitiesCount() <= 0)
			{
				if (_selected.Value.GetEntitiesCount() > 0) return;
				_mediator.Value.SetActiveBonusCard(false);
				return;
			}

			_world = systems.GetWorld();
			EcsWorld message = systems.GetWorld(Idents.Worlds.MessageWorld);

			int bonus = _selected.Value.GetRawEntities()[0];
			message.Add<UpdateItemInfoMessage>(message.NewEntity())
				.Item = _world.PackEntity(_world.Get<Bonus>(bonus).Item);
			_mediator.Value.SetActiveBonusCard(true);
			
			
		}
	}
}