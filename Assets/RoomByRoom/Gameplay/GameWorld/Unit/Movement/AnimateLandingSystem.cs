using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
  public class AnimateLandingSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<UnitViewRef>> _units = default;
    private readonly EcsCustomInject<BlockingService> _blockingSvc = default;

    public void Run(IEcsSystems systems)
    {
      if (_blockingSvc.Value.IsBlocking()) return;

      foreach (int index in _units.Value)
      {
        var groundView = (GroundUnitView)_units.Pools.Inc1.Get(index).Value;
        bool checkSphere = Physics.CheckSphere(groundView.transform.position, 0.1f, groundView.GroundMask,
                                               QueryTriggerInteraction.Ignore);
        if (groundView.Rb.velocity.y < 0 && checkSphere)
          groundView.AnimateJump(false);
      }
    }
  }
}