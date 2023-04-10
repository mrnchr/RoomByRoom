using Leopotam.EcsLite;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class AttackService
	{
		private readonly EcsWorld _world;
		private readonly EcsWorld _message;

		public AttackService(EcsWorld world, EcsWorld message)
		{
			_world = world;
			_message = message;
		}

		public void SetAttackTriggers(int unit, bool isAttack)
		{
			ref MainWeapon mainWeapon = ref _world.GetComponent<MainWeapon>(unit);
			WeaponView weapon = (WeaponView)_world.GetComponent<ItemViewRef>(mainWeapon.Entity).Value;
			weapon.SetActiveAttackTriggers(isAttack);

			if (!isAttack && Utils.IsUnit(_world, unit, UnitType.Humanoid))
			{
				_message.AddComponent<DelayAttackMessage>(_message.NewEntity())
					.Assign(x =>
					{
						x.Unit = unit;
						return x;
					});
			}
		}

		public void Damage(int damaged, int weapon)
		{
			if (!_world.HasComponent<Owned>(weapon))
				return;

			int owner = _world.GetComponent<Owned>(weapon).Owner;
			if (!CanFight(damaged, owner))
				return;

			_message.AddComponent<GetDamageMessage>(_message.NewEntity())
				.Assign(x =>
				{
					x.Damaged = damaged;
					x.Weapon = weapon;
					return x;
				});
		}

		private bool CanFight(int a, int b) => Utils.IsUnit(_world, a, UnitType.Player) || Utils.IsUnit(_world, b, UnitType.Player);
	}
}