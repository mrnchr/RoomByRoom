using System;

using UnityEngine;

namespace RoomByRoom
{
    public class UnitView : View
    {
        [HideInInspector] public Rigidbody Rb;
        public ItemPlace Item;
        [HideInInspector] public Animator Anim;
        [HideInInspector] public AttackCatcher AttackCtr;

        protected virtual void Awake()
        {
            Rb = GetComponent<Rigidbody>();
            Anim = GetComponent<Animator>();
            AttackCtr = GetComponent<AttackCatcher>();
        }

        public virtual void PlayAttackAnimation(WeaponType weaponType)
        {
        }
        
        [Serializable]
        public struct ItemPlace
        {
            public Transform Holder;
            public Transform Point;
        }
    }
}