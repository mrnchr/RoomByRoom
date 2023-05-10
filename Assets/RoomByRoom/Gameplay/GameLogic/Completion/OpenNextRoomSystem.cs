using Leopotam.EcsLite;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
  public class OpenNextRoomSystem : IEcsRunSystem
  {
    private EcsWorld _world;
    private EcsWorld _message;

    public void Run(IEcsSystems systems)
    {
      _world = systems.GetWorld();
      _message = systems.GetWorld(Idents.Worlds.MessageWorld);

      if (_message.Filter<NextRoomMessage>().End().GetEntitiesCount() > 0) return;
      if (_world.Filter<ControllerByAI>().End().GetEntitiesCount() > 0) return;
      int player = Utils.GetPlayerEntity(_world);
      if (_world.Has<Opener>(player)) return;
      Debug.Log("Room is cleaned");
      _message.Add<RoomCleanedMessage>(_message.NewEntity());
      _world.Add<Opener>(player);
    }
  }
}