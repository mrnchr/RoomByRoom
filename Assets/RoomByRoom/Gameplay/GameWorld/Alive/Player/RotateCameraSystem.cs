using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
    public class RotateCameraSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<RotateCameraMessage>> _rotateMessage = Idents.Worlds.MessageWorld;
        private EcsFilterInject<Inc<ControllerByPlayer, UnitViewRef>> _player = default;
        private EcsCustomInject<Configuration> _configuration = default;

        public void Run(IEcsSystems systems)
        {
            foreach(var index in _rotateMessage.Value)
            {
                foreach (var plIndex in _player.Value)
                {
                    ref RotateCameraMessage rotateMessage = ref _rotateMessage.Pools.Inc1.Get(index);
                    PlayerView player = (PlayerView)_player.Pools.Inc2.Get(plIndex).Value;

                    Vector2 rotation = rotateMessage.RotateDirection * _configuration.Value.MouseSensitivity;

                    // rotate camera around player
                    player.CameraHolder.Rotate(rotation.y, 0, 0);
                    player.CameraHolder.Rotate(0, rotation.x, 0, Space.World);
                }
            }
        }
    }
}