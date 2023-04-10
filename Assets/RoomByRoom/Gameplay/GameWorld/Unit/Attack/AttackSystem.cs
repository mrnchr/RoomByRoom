using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class AttackSystem : IEcsRunSystem
	{
		private EcsFilterInject<Inc<AttackCommand, UnitViewRef>> _attacks = default;

		public void Run(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();

			foreach (var attack in _attacks.Value)
			{
				ref UnitViewRef unitViewRef = ref _attacks.Pools.Inc2.Get(attack);
				ref MainWeapon mainWeapon = ref world.GetComponent<MainWeapon>(attack);

				unitViewRef.Value.PlayAttackAnimation(world.GetPool<WeaponInfo>().Get(mainWeapon.Entity).Type);
			}
		}
	}
}