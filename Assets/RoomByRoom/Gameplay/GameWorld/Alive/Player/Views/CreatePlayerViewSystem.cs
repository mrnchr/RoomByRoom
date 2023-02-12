using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
    internal class CreatePlayerViewSystem : IEcsRunSystem
    {
        private EcsCustomInject<PrefabData> _prefabData = default;
        private EcsCustomInject<SavedData> _savedData = default;
        private EcsCustomInject<SceneData> _sceneData = default;
        private EcsFilterInject<Inc<ControllerByPlayer>, Exc<PlayerViewRef>> _player = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            
            foreach(var index in _player.Value)
            {
                RaceType race = _savedData.Value.PlayerRace.Race;
                // Spawn player in the world
                GameObject player = Object.Instantiate(_prefabData.Value.PlayerViews[(int)race].gameObject);
                PlayerView playerView = player.GetComponent<PlayerView>();

                // Add PlayerViewRef component
                ref PlayerViewRef playerRef = ref world.GetPool<PlayerViewRef>().Add(index);
                playerRef.Value = playerView;

                // Update scene data
                _sceneData.Value.PlayerEntity = index;
            }
        }
    }
}