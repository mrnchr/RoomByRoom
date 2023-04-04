using System;

using UnityEngine;

namespace RoomByRoom
{
    [RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(AttackCatcher))]
    public class UnitView : View
    {
        public Moving MovingCmp;
        [HideInInspector] public Animator Anim;
        [HideInInspector] public AttackCatcher AttackCtr;
        [HideInInspector] public Rigidbody Rb;

        protected virtual void Awake()
        {
            Rb = GetComponent<Rigidbody>();
            Anim = GetComponent<Animator>();
            AttackCtr = GetComponent<AttackCatcher>();

            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }

        public virtual void PlayAttackAnimation(WeaponType weaponType)
        {
        }
    }
}