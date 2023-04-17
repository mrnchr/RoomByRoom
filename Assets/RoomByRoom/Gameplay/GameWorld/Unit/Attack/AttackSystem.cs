using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class AttackSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<AttackCommand, UnitViewRef>, Exc<CantAttack>> _attacks = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (int attack in _attacks.Value)
			{
				ref UnitViewRef unitViewRef = ref _attacks.Pools.Inc2.Get(attack);
				unitViewRef.Value.PlayAttackAnimation(GetMainWeaponType(attack));
			}
		}

		private WeaponType GetMainWeaponType(int unit)
		{
			ref MainWeapon mainWeapon = ref _world.GetComponent<MainWeapon>(unit);
			return _world.GetPool<WeaponInfo>().Get(mainWeapon.Entity).Type;
		}
	}
}