using System.Collections;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
    public class JumpUnitSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<Jumping, Moving, JumpCommand>> _units = default;

        public void Run(IEcsSystems systems)
        {
            foreach(var index in _units.Value)
            {
                ref Moving moving = ref _units.Pools.Inc2.Get(index);
                ref Jumping jumping = ref _units.Pools.Inc1.Get(index);

                if(jumping.CanJump && Physics.CheckSphere(moving.Rb.transform.position, 0.01f, jumping.GroundMask, QueryTriggerInteraction.Ignore))
                {
                    Vector3 dir = moving.Rb.velocity;
                    dir.y = 0;
                    moving.Rb.velocity = dir;
                    moving.Rb.AddForce(Vector3.up * jumping.JumpForce, ForceMode.Impulse);
                    jumping.CanJump = false;
                }
            }
        }
    }
}