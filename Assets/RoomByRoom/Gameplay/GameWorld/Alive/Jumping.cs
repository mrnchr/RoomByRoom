using System;
using UnityEngine;

namespace RoomByRoom
{
    [Serializable]
    public struct Jumping
    {
        public float JumpForce;
        public LayerMask GroundMask;
        [NonSerialized] public bool CanJump;
    }
}