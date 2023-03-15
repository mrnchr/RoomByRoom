using UnityEngine;

namespace RoomByRoom
{
    public class UnitView : View
    {
        public Moving Moving;
        public Rigidbody Rb;
        public Animator Animator;

        public virtual void PlayAttackAnimation(WeaponType weaponType)
        {
        }

        public void OnReset()
        {
            TryGetComponent<Animator>(out Animator);
        }
    }
}