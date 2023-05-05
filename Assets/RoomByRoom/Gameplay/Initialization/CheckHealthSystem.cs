using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Gameplay.GameWorld.Unit.Player;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class CheckHealthSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<Health, UnitInfo>, Exc<Died>> _units = default;
		private EcsWorld _world;
		private EcsWorld _message;
		
		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_message = systems.GetWorld(Idents.Worlds.MessageWorld);
			
			foreach (int index in _units.Value)
			{
				if (_world.Get<Health>(index).CurrentPoint > 0) continue;
				if (Utils.IsPlayer(_world, index))
				{
					_message.Add<PlayerDyingMessage>(_message.NewEntity());
					_world.Add<Died>(index);
				}
				else
				{
					_world.Add<DieCommand>(index);
				}
			}
		}
	}
}