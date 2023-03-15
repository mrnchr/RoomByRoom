using UnityEngine;

namespace RoomByRoom
{
    public class HumanoidView : GroundUnitView
    {
        public ItemView MainWeapon;

        public override void PlayAttackAnimation(WeaponType weaponType)
        {            
            Animator.SetInteger("Weapon", (int)weaponType);
            Animator.SetTrigger("StartAttack");

            // TODO: change when there is a bow
        }
    }
}