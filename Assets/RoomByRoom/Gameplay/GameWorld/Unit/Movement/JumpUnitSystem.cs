using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class JumpUnitSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<Jumpable, UnitViewRef, JumpCommand>, Exc<CantJump>> _units = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            foreach(var index in _units.Value)
            {
                GroundUnitView groundUnit = (GroundUnitView)(world.GetComponent<UnitViewRef>(index).Value);
                ref Jumpable jumpable = ref world.GetComponent<Jumpable>(index);

                if(Physics.CheckSphere(groundUnit.transform.position, 0.01f, groundUnit.GroundMask, QueryTriggerInteraction.Ignore))
                {
                    groundUnit.Rb.velocity = Vector3.Scale(groundUnit.Rb.velocity, new Vector3(1, 0, 1));
                    groundUnit.Rb.AddForce(Vector3.up * jumpable.JumpForce, ForceMode.Impulse);
                    world.AddComponent<CantJump>(index);
                }
            }
        }
    }
}