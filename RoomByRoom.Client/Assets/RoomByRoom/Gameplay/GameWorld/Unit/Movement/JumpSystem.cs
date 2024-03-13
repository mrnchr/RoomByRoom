using System.Collections;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
  public class JumpSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<JumpCommand>, Exc<CantJump>> _units = default;
    private EcsWorld _world;

    public void Run(IEcsSystems systems)
    {
      _world = systems.GetWorld();

      foreach (int index in _units.Value)
      {
        var groundView = (GroundUnitView)_world.Get<UnitViewRef>(index).Value;
        bool checkSphere = Physics.CheckSphere(groundView.transform.position, 0.01f, groundView.GroundMask);

        if (!checkSphere) continue;
        float jumpForce = _world.Get<Jumpable>(index).JumpForce;
        groundView.Rb.velocity = ClearVertical(groundView.Rb.velocity);
        groundView.Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        groundView.AnimateJump(true);
        _world.Add<CantJump>(index)
          .TimeLeft = 0.2f;
      }
    }

    private static Vector3 ClearVertical(Vector3 velocity) => Vector3.Scale(velocity, new Vector3(1, 0, 1));
  }
}