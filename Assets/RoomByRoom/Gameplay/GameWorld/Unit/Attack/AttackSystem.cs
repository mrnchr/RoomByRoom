using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Config.Data;
using RoomByRoom.Utility;

namespace RoomByRoom
{
  public class AttackSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<AttackCommand, UnitViewRef>, Exc<CantAttack>> _attacks = default;
    private readonly EcsCustomInject<EnemyData> _enemyData = default;

    public void Run(IEcsSystems systems)
    {
      EcsWorld world = systems.GetWorld();

      foreach (int attack in _attacks.Value)
      {
        ref UnitViewRef unitViewRef = ref _attacks.Pools.Inc2.Get(attack);
        unitViewRef.Value.StartAttackAnimation();
        
        if(Utils.IsUnitOf(world, attack, UnitType.Humanoid))
          world.Add<CantAttack>(attack)
            .TimeLeft = _enemyData.Value.Armor.CantRestoreTime;
      }
    }
  }
}