using System.Linq;
using Leopotam.EcsLite;
using RoomByRoom.Utility;

namespace RoomByRoom
{
  public class CharacteristicService
  {
    private readonly EcsWorld _world;

    public CharacteristicService(EcsWorld world)
    {
      _world = world;
    }

    public void Calculate(int unit) => CalcPhysProtection(unit);

    private void CalcPhysProtection(int unit)
    {
      float totalProtection = _world.Get<Equipment>(unit).ItemList
        .Select(x => _world.Unpack(x))
        .Where(x => _world.Has<ItemPhysicalProtection>(x))
        .Sum(x => _world.Get<ItemPhysicalProtection>(x).Point);

      // UnityEngine.Debug.Log($"Entity: {unit}, protection: {totalProtection}");
      _world.Get<UnitPhysicalProtection>(unit)
        .MaxPoint = totalProtection;
    }
  }
}