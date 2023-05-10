using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
  public class ReloadGameSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<PlayerDyingMessage>> _dieMsgs = Idents.Worlds.MessageWorld;
    private readonly EcsCustomInject<ReloadSceneService> _sceneSvc = default;
    private readonly EcsCustomInject<SavingService> _savingSvc = default;

    public void Run(IEcsSystems systems)
    {
      foreach (int _ in _dieMsgs.Value)
      {
        OuterData outerData = new GameObject().AddComponent<OuterData>();
        outerData.ProfileName = _savingSvc.Value.ProfileName;
        Object.DontDestroyOnLoad(outerData);
        
        _sceneSvc.Value.ReloadScene();
      }
    }
  }
}