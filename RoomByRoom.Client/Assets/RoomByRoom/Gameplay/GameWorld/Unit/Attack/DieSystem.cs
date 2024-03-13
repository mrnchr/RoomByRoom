using System.Linq;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
  public class DieSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<DieCommand>> _units = default;

    public void Run(IEcsSystems systems)
    {
      EcsWorld world = systems.GetWorld();

      foreach (int index in _units.Value)
      {
        // Debug.Log($"Entity {index} died");
        foreach (int item in world.Get<Equipment>(index).ItemList.Select(x => world.Unpack(x)))
        {
          Object.Destroy(world.Get<ItemViewRef>(item).Value.gameObject);
          world.DelEntity(item);
        }

        Object.Destroy(world.Get<UnitViewRef>(index).Value.gameObject);
        world.DelEntity(index);
      }
    }
  }
}