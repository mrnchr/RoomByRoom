using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    internal class PutPlayerInRoomSystem : IEcsRunSystem
    {
        private EcsCustomInject<SceneData> _sceneData = default;
        private EcsFilterInject<Inc<UnitViewRef, ControllerByPlayer>> _playerViewRef = default;
        private EcsFilterInject<Inc<NoPlayer, RoomViewRef>> _rooms = default;

        public void Run(IEcsSystems systems)
        {
            // if the player or the room isn't spawned return
            if(_playerViewRef.Value.GetEntitiesCount() == 0)
                return;
            foreach (var index in _rooms.Value)
            {
                // UnityEngine.Debug.Log("Put player in room");

                // teleport the player into the room
                ref UnitViewRef player = ref _playerViewRef.Pools.Inc1.Get(_sceneData.Value.PlayerEntity);
                ref RoomViewRef room = ref _rooms.Pools.Inc2.Get(index);

                // UnityEngine.Debug.Log($"~Player: {player.Value.transform.position}. SpawnPoint: {room.Value.SpawnPoint.position}");
                player.Value.transform.position = room.Value.SpawnPoint.position;
            }
        }
    }
}