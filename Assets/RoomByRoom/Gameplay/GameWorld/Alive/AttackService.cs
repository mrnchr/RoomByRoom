using UnityEngine;

using Leopotam.EcsLite;

namespace RoomByRoom
{
    public class AttackService
    {
        private EcsWorld _world;

        public AttackService(EcsWorld world)
        {
            _world = world;
        }

        public void SetAttackTriggers(int unit, bool isAttack)
        {
            ref MainWeapon mainWeapon = ref _world.GetPool<MainWeapon>().Get(unit);
            ref ItemViewRef weaponRef = ref _world.GetPool<ItemViewRef>().Get(mainWeapon.Entity);
            WeaponView weapon = (WeaponView)weaponRef.Value;
            weapon.SetActiveAttackTriggers(isAttack);
        }   

        public void Damage(int damaging, int damaged)
        {
            // player & enemy or player & boss
            if(!CanFight(damaging, damaged))
                return;

            ref MainWeapon mainWeapon = ref _world.GetPool<MainWeapon>().Get(damaging);
            ref GetDamageCommand damageCmd = ref _world.GetPool<GetDamageCommand>().Add(damaged);
            damageCmd.Weapon = mainWeapon.Entity;
        }

        public bool CanFight(int a, int b)
        {
            ref UnitInfo aUnit = ref _world.GetPool<UnitInfo>().Get(a);
            ref UnitInfo bUnit = ref _world.GetPool<UnitInfo>().Get(b);

            return aUnit.Type == UnitType.Player || bUnit.Type == UnitType.Player;
        }
    }
}