using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

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
            int entity = _message.NewEntity();
            _message.AddComponent<RotateCameraMessage>(entity)
                .Initialize(x => { x.RotateDirection = GetRotationInput(); return x; });
        }

        private static Vector2Int GetRotationInput()
        {
            return new Vector2Int((int)Input.GetAxisRaw("Mouse X"), (int)Input.GetAxisRaw("Mouse Y"));
        }

        private void Jump(int entity)
        {
            if(Input.GetAxisRaw("Jump") > 0)
                _world.AddComponent<JumpCommand>(entity);
        }

        private void Move(int entity)
        {
            _world.AddComponent<MoveCommand>(entity)
                .Initialize(x => { x.MoveDirection = GetMovementInput(); return x; });
        }

        private static Vector3Int GetMovementInput()
        {
            return new Vector3Int((int)Input.GetAxisRaw("Horizontal"), 0, (int)Input.GetAxisRaw("Vertical"));
        }

        private void OpenDoor()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                int entity = _message.NewEntity();
                _message.AddComponent<OpenDoorMessage>(entity);
            }
        }

        private void Attack(int entity)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
                _world.AddComponent<AttackCommand>(entity);
        }
    }
}