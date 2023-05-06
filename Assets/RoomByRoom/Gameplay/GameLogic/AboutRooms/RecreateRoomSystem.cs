using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
  public class RecreateRoomSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<NextRoomMessage>> _nextRoomMsg = Idents.Worlds.MessageWorld;
    private readonly EcsFilterInject<Inc<RoomViewRef>> _room = default;
    private EcsWorld _world;

    public void Run(IEcsSystems systems)
    {
      _world = systems.GetWorld();
      EcsWorld message = systems.GetWorld(Idents.Worlds.MessageWorld);

      foreach (int index in _nextRoomMsg.Value)
      {
        DeleteRoom();
        CreateRoom(message.Get<NextRoomMessage>(index));
      }
    }

    private void CreateRoom(NextRoomMessage nextRoom)
    {
      int roomEntity = _world.NewEntity();

      _world.Add<RaceInfo>(roomEntity) = nextRoom.Race;
      _world.Add<RoomInfo>(roomEntity) = nextRoom.Room;
    }

    private void DeleteRoom()
    {
      int roomEntity = _room.Value.GetRawEntities()[0];
      Object.Destroy(_world.Get<RoomViewRef>(roomEntity).Value.gameObject);
      _world.DelEntity(roomEntity);
    }
  }
}