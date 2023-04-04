using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
    public class RotateCameraSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<RotateCameraMessage>> _rotateCameraMsgs = Idents.Worlds.MessageWorld;
        private EcsFilterInject<Inc<ControllerByPlayer, UnitViewRef>> _player = default;
        private EcsCustomInject<Configuration> _configuration = default;

        public void Run(IEcsSystems systems)
        {
            int playerEntity = _player.Value.GetRawEntities()[0];

            foreach(var index in _rotateCameraMsgs.Value)
            {
                ref RotateCameraMessage rotateMessage = ref _rotateCameraMsgs.Pools.Inc1.Get(index);
                PlayerView player = (PlayerView)_player.Pools.Inc2.Get(playerEntity).Value;

                Vector2 rotation = rotateMessage.RotateDirection * _configuration.Value.MouseSensitivity;

                player.CameraHolder.Rotate(rotation.y, 0, 0);
                player.CameraHolder.Rotate(0, rotation.x, 0, Space.World);
            }
        }
    }
}