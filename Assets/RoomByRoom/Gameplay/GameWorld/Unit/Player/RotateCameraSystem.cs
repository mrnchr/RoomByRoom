using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Control;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
  public class RotateCameraSystem : IEcsRunSystem
  {
    private readonly EcsCustomInject<Configuration> _config = default;
    private readonly EcsFilterInject<Inc<ControllerByPlayer, UnitViewRef>> _player = default;
    private readonly EcsFilterInject<Inc<RotateCameraMessage>> _rotateCameraMsgs = Idents.Worlds.MessageWorld;
    private float _xRotation;

    public void Run(IEcsSystems systems)
    {
      int playerEntity = _player.Value.GetRawEntities()[0];

      foreach (int index in _rotateCameraMsgs.Value)
      {
        var player = (PlayerView)_player.Pools.Inc2.Get(playerEntity).Value;

        Vector2 rotation = CalculateRotation(index);

        player.CameraHolder.Rotate(-rotation.y, 0, 0);
        player.CameraHolder.Rotate(0, rotation.x, 0, Space.World);
      }
    }

    private Vector2 CalculateRotation(int message)
    {
      ref RotateCameraMessage rotateMessage = ref _rotateCameraMsgs.Pools.Inc1.Get(message);
      Vector2 rotation = rotateMessage.RotateDirection * _config.Value.MouseSensitivity;
      _xRotation -= rotation.y;
      if (_xRotation is < -60 or > 60)
      {
        rotation.y = 0;
        _xRotation = Mathf.Clamp(_xRotation, -60, 60);
      }

      return rotation;
    }
  }
}