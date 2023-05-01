using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class AttackSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<AttackCommand, UnitViewRef>, Exc<CantAttack>> _attacks = default;

		public void Run(IEcsSystems systems)
		{
			foreach (int attack in _attacks.Value)
			{
				ref UnitViewRef unitViewRef = ref _attacks.Pools.Inc2.Get(attack);
				unitViewRef.Value.StartAttackAnimation();
			}
		}
	}
}