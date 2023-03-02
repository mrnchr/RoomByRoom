using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
    public class InputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterInject<Inc<ControllerByPlayer>> _controller = default;
        private EcsWorld _world;
        private EcsWorld _message;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _message = systems.GetWorld(Idents.Worlds.MessageWorld);
        }

        public void Run(IEcsSystems systems)
        {

            foreach(var index in _controller.Value)
            {
                OpenDoor();
                Move(index);
                Jump(index);
                RotateCamera();
                Attack(index);
            }
        }

        private void RotateCamera()
        {
            // Send RotateCameraMessage
            int entity = _message.NewEntity();
            ref RotateCameraMessage command = ref _message.GetPool<RotateCameraMessage>().Add(entity);
            command.RotateDirection = new Vector2Int((int)Input.GetAxisRaw("Mouse X"), (int)Input.GetAxisRaw("Mouse Y"));
        }

        private void Jump(int entity)
        {
            if(Input.GetAxisRaw("Jump") > 0)
                _world.GetPool<JumpCommand>().Add(entity);
        }

        private void Move(int entity)
        {
            // Read input
            var moveDirection = new Vector3Int
            (
                (int)Input.GetAxisRaw("Horizontal"), 
                0, 
                (int)Input.GetAxisRaw("Vertical")
            );
        
            // Add MoveCommand component
            ref MoveCommand moveCommand = ref _world.GetPool<MoveCommand>().Add(entity);
            moveCommand.MoveDirection = moveDirection;
        }

        private void OpenDoor()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Send OpenDoorMessage
                int entity = _message.NewEntity();
                _message.GetPool<OpenDoorMessage>().Add(entity);
            }
        }

        private void Attack(int entity)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                // Add AttackCommand component
                _world.GetPool<AttackCommand>().Add(entity);
            }
        }
    }
}