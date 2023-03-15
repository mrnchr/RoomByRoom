using System.Collections;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
    public class JumpUnitSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<Jumping, UnitViewRef, JumpCommand>, Exc<CantJump, Flying>> _units = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            foreach(var index in _units.Value)
            {
                GroundUnitView groundUnit = (GroundUnitView)(_units.Pools.Inc2.Get(index).Value);
                ref Jumping jumping = ref _units.Pools.Inc1.Get(index);

                if(Physics.CheckSphere(groundUnit.transform.position, 0.01f, groundUnit.GroundMask, QueryTriggerInteraction.Ignore))
                {
                    groundUnit.Rb.velocity.Scale(new Vector3(1, 0, 1));
                    groundUnit.Rb.AddForce(Vector3.up * jumping.JumpForce, ForceMode.Impulse);
                    world.GetPool<CantJump>().Add(index);
                }
            }
        }
    }
}