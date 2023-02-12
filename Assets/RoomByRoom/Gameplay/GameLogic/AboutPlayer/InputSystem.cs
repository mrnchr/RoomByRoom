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

            foreach(var index in _controller.Value)
            {
                if(Input.GetKeyDown(KeyCode.F))
                {
                    // Send OpenDoorMessage
                    int entity = message.NewEntity();
                    message.GetPool<OpenDoorMessage>().Add(entity);
                }
            }
        }
    }
}