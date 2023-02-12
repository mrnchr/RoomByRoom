using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom 
{
    sealed class BootStrap : MonoBehaviour 
    {
        [SerializeField] private SceneData _sceneData;
        [SerializeField] private SavedData _savedData;
        [SerializeField] private PrefabData _prefabData;
        EcsWorld _world;        
        IEcsSystems _updateSystems;

        void Start () {
            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world);
            _updateSystems
                .Add(new LoadRoomSystem())
                .Add(new CreateRoomViewSystem())
                .Add(new LoadPlayerSystem())
                .Add(new CreatePlayerViewSystem())
                .Add(new PutPlayerInRoomSystem())
                .Add(new InputSystem())
                .Add(new OpenDoorSystem())
                .Add(new RandomRoomSystem())
                .Add(new RecreateRoomSystem())
                .DelHere<NoPlayer>()

                .AddWorld(new EcsWorld(), Idents.Worlds.MessageWorld)
                .DelHere<NextRoomMessage>(Idents.Worlds.MessageWorld)
                .DelHere<OpenDoorMessage>(Idents.Worlds.MessageWorld)
                .DelHere<AddPlayerMessage>(Idents.Worlds.MessageWorld)
                .DelHere<StartGameMessage>(Idents.Worlds.MessageWorld)
#if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem (Idents.Worlds.MessageWorld))
#endif
                .Inject(_sceneData, _savedData, _prefabData)
                .Init();
        }

        void Update () 
        {
            _updateSystems?.Run();
        }

        void OnDestroy () {
            if (_updateSystems != null) 
            {
                _updateSystems.Destroy();
                _updateSystems = null;
            }

            
            
            if (_world != null) 
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}