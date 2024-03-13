using Infrastructure.SceneLoading;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class EntryPoint : MonoBehaviour, IInitializable
    {
        private ISceneLoadingService _sceneSvc;

        [Inject]
        public void Construct(ISceneLoadingService sceneSvc)
        {
            _sceneSvc = sceneSvc;
        }   
        
        public void Initialize()
        {
            _sceneSvc.LoadSignScene();
        }
    }
}