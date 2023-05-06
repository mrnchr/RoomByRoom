using Leopotam.EcsLite;

namespace RoomByRoom.Testing
{
  public interface ICreator
  {
    public int CreateEntity(EcsWorld world);
  }
}