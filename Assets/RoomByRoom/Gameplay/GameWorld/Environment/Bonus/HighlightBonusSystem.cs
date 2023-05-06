using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
  public class HighlightBonusSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<DeselectCommand>> _deselected = default;
    private readonly EcsFilterInject<Inc<SelectCommand>> _selected = default;
    private EcsWorld _world;

    public void Run(IEcsSystems systems)
    {
      _world = systems.GetWorld();

      foreach (int index in _selected.Value)
        Highlight(index, true);

      foreach (int index in _deselected.Value)
        Highlight(index, false);
    }

    private void Highlight(int index, bool highlight)
    {
      Renderer renderer = _world.Get<BonusViewRef>(index).Value.Shell;
      Color newColor = renderer.material.color;
      newColor.a = highlight ? 0.5f : 0;
      renderer.material.color = newColor;
    }
  }
}