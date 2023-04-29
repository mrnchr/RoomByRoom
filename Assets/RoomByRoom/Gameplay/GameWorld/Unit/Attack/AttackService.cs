using Leopotam.EcsLite;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class AttackService
	{
		private readonly EcsWorld _message;
		private readonly EcsWorld _world;

		public AttackService(EcsWorld world, EcsWorld message)
		{
			_world = world;
			_message = message;
		}

		public void SetAttackTriggers(int unit, bool isAttack)
		{
			ref MainWeapon mainWeapon = ref _world.Get<MainWeapon>(unit);
			var weapon = (WeaponView)_world.Get<ItemViewRef>(mainWeapon.Entity).Value;
			weapon.SetActiveAttackTriggers(isAttack);

			if (!isAttack && Utils.IsUnitOf(_world, unit, UnitType.Humanoid))
				_message.Add<DelayAttackMessage>(_message.NewEntity())
					.Assign(x =>
					{
						x.Unit = unit;
						return x;
					});
		}

		public void Damage(int damaged, int weapon)
		{
			if (!_world.Has<Owned>(weapon))
				return;

			int owner = _world.Get<Owned>(weapon).Owner;
			if (!CanFight(damaged, owner))
				return;

			_message.Add<GetDamageMessage>(_message.NewEntity())
				.Assign(x =>
				{
					x.Damaged = damaged;
					x.Weapon = weapon;
					return x;
				});
		}

		private bool CanFight(int a, int b) =>
			Utils.IsUnitOf(_world, a, UnitType.Player) || Utils.IsUnitOf(_world, b, UnitType.Player);
	}
}