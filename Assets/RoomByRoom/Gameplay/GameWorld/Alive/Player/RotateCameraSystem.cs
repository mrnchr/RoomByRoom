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
        private EcsCustomInject<SceneData> _sceneData = default;

        public void Run(IEcsSystems systems)
        {
            foreach(var index in _rotateMessage.Value)
            {
                ref RotateCameraMessage rotateMessage = ref _rotateMessage.Pools.Inc1.Get(index);
                PlayerView player = (PlayerView)_player.Pools.Inc2.Get(_sceneData.Value.PlayerEntity).Value;
                Vector3 vel = player.Rb.angularVelocity;

                // assign to angular velocity the value according to RotateCameraMessage
                vel.y = rotateMessage.RotateDirection.x * _configuration.Value.MouseSensitivity.x;
                player.Rb.angularVelocity = vel;

                // rotate camera around the player
                player.MainCamera.RotateAround
                (
                    player.transform.position,
                    player.transform.right,
                    rotateMessage.RotateDirection.y * _configuration.Value.MouseSensitivity.y
                );
            }
        }
    }
}