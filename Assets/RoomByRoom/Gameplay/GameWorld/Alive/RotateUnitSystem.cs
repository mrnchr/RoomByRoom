using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
    public class RotateUnitSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<MoveCommand, UnitViewRef>> _units = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            foreach(var index in _units.Value)
            {
                ref MoveCommand command = ref _units.Pools.Inc1.Get(index);
                Vector3Int moveDirection = command.MoveDirection;

                if(moveDirection != Vector3Int.zero)
                {
                    Transform unitTransform;
                    ref UnitViewRef unitView = ref _units.Pools.Inc2.Get(index);

                    // if it's player then the transform is his mesh
                    if(world.GetPool<ControllerByPlayer>().Has(index))
                    {
                        unitTransform = ((PlayerView)unitView.Value).Character;
                    }
                    else 
                    {
                        unitTransform = unitView.Value.transform;
                    }

                    // if jumping i.e. not flying, to change only two axis 
                    unitTransform.forward = new Vector3
                    (
                        moveDirection.x, 
                        world.GetPool<Jumping>().Has(index) ? moveDirection.y : 0, 
                        moveDirection.z
                    )
                    .normalized;
                }
            }
        }
    }
}