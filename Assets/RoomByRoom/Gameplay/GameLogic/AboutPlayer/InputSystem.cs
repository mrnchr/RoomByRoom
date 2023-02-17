using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
    internal class InputSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<ControllerByPlayer>> _controller = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld message = systems.GetWorld(Idents.Worlds.MessageWorld);
            EcsWorld world = systems.GetWorld();

            foreach(var index in _controller.Value)
            {
                OpenDoor(message);
                Move(world, index);
                Jump(world, index);
                RotateCamera(message);
            }
        }

        private void RotateCamera(EcsWorld message)
        {
            // Send RotateCameraMessage
            int entity = message.NewEntity();
            ref RotateCameraMessage command = ref message.GetPool<RotateCameraMessage>().Add(entity);
            command.RotateDirection = new Vector2Int((int)Input.GetAxisRaw("Mouse X"), (int)Input.GetAxisRaw("Mouse Y"));
        }

        private void Jump(EcsWorld world, int entity)
        {
            if(Input.GetAxisRaw("Jump") > 0)
                world.GetPool<JumpCommand>().Add(entity);
        }

        private void Move(EcsWorld world, int entity)
        {
            // Read input
            var moveDirection = new Vector3Int
            (
                (int)Input.GetAxisRaw("Horizontal"), 
                0, 
                (int)Input.GetAxisRaw("Vertical")
            );
        
            // Add MoveCommand component
            ref MoveCommand moveCommand = ref world.GetPool<MoveCommand>().Add(entity);
            moveCommand.MoveDirection = moveDirection;
        }

        private void OpenDoor(EcsWorld message)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Send OpenDoorMessage
                int entity = message.NewEntity();
                message.GetPool<OpenDoorMessage>().Add(entity);
            }
        }
    }
}