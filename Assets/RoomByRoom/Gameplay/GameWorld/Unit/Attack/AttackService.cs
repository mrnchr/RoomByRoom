using Leopotam.EcsLite;

using RoomByRoom.Utility;

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
            ref MainWeapon mainWeapon = ref _world.GetComponent<MainWeapon>(unit);
            WeaponView weapon = (WeaponView)_world.GetComponent<ItemViewRef>(mainWeapon.Entity).Value;
            weapon.SetActiveAttackTriggers(isAttack);
        }   

        public void Damage(int damaging, int damaged)
        {
            if(!CanFight(damaging, damaged))
                return;

            MainWeapon mainWeapon = _world.GetComponent<MainWeapon>(damaging);
            _world.AddComponent<GetDamageCommand>(damaged)
                .Initialize(x => { x.Entity = mainWeapon.Entity; return x; });
        }

        protected bool CanFight(int a, int b)
        {
            return IsPlayer(a) || IsPlayer(b);
        }

        protected bool IsPlayer(int entity) => GetUnitType(entity) == UnitType.Player;
        protected UnitType GetUnitType(int entity) => _world.GetComponent<UnitInfo>(entity).Type;


    }
}