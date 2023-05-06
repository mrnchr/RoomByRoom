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
      int mainWeapon = _world.Get<MainWeapon>(unit).Entity;
      if (_world.Has<ItemViewRef>(mainWeapon))
      {
        var weapon = (WeaponView)_world.Get<ItemViewRef>(mainWeapon).Value;
        weapon.SetActiveAttackTriggers(isAttack);
      }

      if (!isAttack && Utils.IsUnitOf(_world, unit, UnitType.Humanoid))
        _message.Add<DelayAttackMessage>(_message.NewEntity())
          .Unit = unit;
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
      Utils.IsPlayer(_world, a) || Utils.IsPlayer(_world, b);
  }
}