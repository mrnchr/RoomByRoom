using UnityEngine;

namespace RoomByRoom
{
    public class UnitView : MonoBehaviour
    {
        public Moving Moving;
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