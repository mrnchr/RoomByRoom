using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
    internal class CreatePlayerViewSystem : IEcsRunSystem
    {
        private EcsCustomInject<PackedPrefabData> _packedPrefabData = default;
        private EcsCustomInject<SavedData> _savedData = default;
        private EcsCustomInject<SceneData> _sceneData = default;
        private EcsFilterInject<Inc<ControllerByPlayer>, Exc<UnitViewRef>> _player = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            
            foreach(var index in _player.Value)
            {
                ref PlayerEntity playerEntity = ref _savedData.Value.Player;
                // Spawn player in the world
                GameObject player = Object.Instantiate(_packedPrefabData.Value.Prefabs.BasePlayerUnit.gameObject);
                PlayerView playerView = player.GetComponent<PlayerView>();

                // Add PlayerViewRef component
                ref UnitViewRef playerRef = ref world.GetPool<UnitViewRef>().Add(index);
                playerRef.Value = playerView;

                // Add Moving component
                ref Moving moving = ref world.GetPool<Moving>().Add(index);
                moving = playerEntity.Moving;

                // Add Jumping component
                ref Jumping jumping = ref world.GetPool<Jumping>().Add(index);
                jumping = playerEntity.Jumping;

                // Update scene data
                _sceneData.Value.PlayerEntity = index;
            }
        }
    }
}