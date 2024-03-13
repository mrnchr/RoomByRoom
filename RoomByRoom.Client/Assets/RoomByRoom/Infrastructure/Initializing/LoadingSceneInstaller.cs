using Infrastructure.SceneLoading;
using UI.SceneLoading;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class LoadingSceneInstaller : MonoInstaller
    {
        [SerializeField] private LoadingView _view;
        
        public override void InstallBindings()
        {
            BindSceneLoadingModel();
            BindSceneLoader();
            BindLoadingController();
        }

        private void BindSceneLoader()
        {
            Container
                .BindInterfacesTo<SceneLoader>()
                .AsSingle();
        }

        private void BindSceneLoadingModel()
        {
            Container
                .Bind<SceneLoadingModel>()
                .AsSingle();
        }

        private void BindLoadingController()
        {
            Container
                .Bind<ILoadingController>()
                .To<LoadingController>()
                .AsSingle()
                .WithArguments(_view);
        }
    }
}