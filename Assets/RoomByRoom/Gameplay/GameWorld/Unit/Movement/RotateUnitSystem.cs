using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

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
                Vector3Int moveDirection = world.GetComponent<MoveCommand>(index).MoveDirection;

                if (IsMoving(moveDirection))
                {
                    UnitView unitView = world.GetComponent<UnitViewRef>(index).Value;

                    if (unitView is PlayerView player)
                        RotatePlayer(moveDirection, player);
                    else
                        RotateUnit(moveDirection, unitView);
                }
            }
        }

        private bool IsMoving(Vector3Int moveDirection) => moveDirection != Vector3Int.zero;

        private void RotateUnit(Vector3Int moveDirection, UnitView unitView)
        {
            unitView.transform.forward = ((Vector3)moveDirection).normalized;
        }

        private void RotatePlayer(Vector3Int moveDirection, PlayerView player)
        {
            Vector3 forward = player.CameraHolder.TransformDirection(moveDirection);
            forward.y = 0;
            player.Character.forward = forward.normalized;
        }
    }
}