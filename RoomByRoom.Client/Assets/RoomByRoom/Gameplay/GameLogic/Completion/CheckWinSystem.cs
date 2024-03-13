using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
  public class CheckWinSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<NextRoomMessage>> _nextRoomMsgs = Idents.Worlds.MessageWorld;
    private readonly EcsCustomInject<ProgressData> _progress = default;
    private readonly EcsCustomInject<GlobalState> _globalState = default;
    private EcsWorld _message;

    public void Run(IEcsSystems systems)
    {
      _message = systems.GetWorld(Idents.Worlds.MessageWorld);
      foreach (int _ in _nextRoomMsgs.Value)
      {
        if (_progress.Value.Game.RoomCount > 10)
        {
          _progress.Value.Game.RoomCount = 10;
          _globalState.Value.IsWin = true;
          _message.Add<WinMessage>(_message.NewEntity());
        }
      }
    }
  }
}