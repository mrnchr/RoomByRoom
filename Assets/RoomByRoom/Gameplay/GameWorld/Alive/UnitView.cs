using System;

using UnityEngine;

namespace RoomByRoom
{
    public class UnitView : View
    {
        public Rigidbody Rb;
        public ItemPlace Item;
        public Animator Animator;

        public virtual void PlayAttackAnimation(WeaponType weaponType)
        {
        }

        public void OnReset()
        {
            TryGetComponent<Animator>(out Animator);
        }
        
        [Serializable]
        public struct ItemPlace
        {
            public Transform Holder;
            public Transform Point;
        }
    }
}