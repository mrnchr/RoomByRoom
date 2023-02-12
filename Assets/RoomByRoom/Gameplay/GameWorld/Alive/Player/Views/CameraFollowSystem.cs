using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
    public class CameraFollowSystem : IEcsPostRunSystem
    {
        private EcsFilterInject<Inc<ControllerByPlayer, UnitViewRef>> _player = default;

        public void PostRun(IEcsSystems systems)
        {
            foreach(var index in _player.Value)
            {
                ref UnitViewRef player = ref _player.Pools.Inc2.Get(index);
            }
        }
    }
}