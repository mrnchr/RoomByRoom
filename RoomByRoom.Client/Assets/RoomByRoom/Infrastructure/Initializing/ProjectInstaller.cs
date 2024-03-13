using Authorization;
using Configuration;
using Infrastructure.GameCycle;
using Infrastructure.Logging;
using Infrastructure.SceneLoading;
using Profile;
using RoomByRoom;
using RoomByRoom.Web.Building;
using RoomByRoom.Web.Low;
using RoomByRoom.Web.Processor;
using RoomByRoom.Web.RequestService;
using RoomByRoom.Web.Sender;
using RoomByRoom.Web.Utils;
using RoomByRoom.Web.WebService;
using UI.Profile;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ConfigProvider _configProvider;
        [SerializeField] private CoroutineRunner _runner;
        [SerializeField] private WebConfig _config;
        
        public override void InstallBindings()
        {
            BindConfigProvider();
            BindWebConfig();

            BindCoroutineRunner();
            BindPersonalLogger();
            BindTokenProvider();
            BindWebRequest();
            BindWebRequestSenderFactory();
            BindAuthorizationSetter();
            BindWebRequestBuilder();
            BindWebRequestService();
            BindWebRequestProcessor();
            BindWebService();

            BindSceneLoader();
            BindSceneLoadingService();
            
            BindAuthorizationWebService();

            BindProgressData();
            BindProfileWebService();
            BindProgressSaver();
            BindGameSaveService();
            
            BindGameStateFactory();
            BindGameStateMachine();
        }

        private void BindWebConfig()
        {
            WebConfig config = new WebConfigConverter().Convert();
            _config = config;
            Container
                .BindInstance(config)
                .AsSingle();
        }

        private void BindAuthorizationWebService()
        {
            Container
                .Bind<IAuthenticationWebService>()
                .To<AuthenticationWebService>()
                .AsSingle();
        }

        private void BindGameSaveService()
        {
            Container
                .Bind<IGameSaveService>()
                .To<GameSaveService>()
                .AsSingle();
        }

        private void BindProgressData()
        {
            Container
                .Bind<ProgressData>()
                .AsSingle();
        }

        private void BindProgressSaver()
        {
            Container
                .Bind<ISaver>()
                .To<ProgressSaver>()
                .AsSingle();
        }

        private void BindProfileWebService()
        {
            Container
                .Bind<IProfileWebService>()
                .To<ProfileWebService>()
                .AsSingle();
        }

        private void BindSceneLoadingService()
        {
            Container
                .Bind<ISceneLoadingService>()
                .To<SceneLoadingService>()
                .AsSingle();
        }

        private void BindSceneLoader()
        {
            Container
                .Bind<ISceneTransition>()
                .To<SceneTransition>()
                .AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container
                .BindInterfacesTo<GameStateMachine>()
                .AsSingle();
        }

        private void BindGameStateFactory()
        {
            Container
                .Bind<IGameStateFactory>()
                .To<GameStateFactory>()
                .AsSingle();
        }

        private void BindWebService()
        {
            Container
                .Bind<IWebService>()
                .To<WebService>()
                .AsSingle();
        }

        private void BindWebRequestProcessor()
        {
            Container
                .BindInterfacesTo<WebRequestProcessor>()
                .AsSingle();
        }

        private void BindWebRequestService()
        {
            Container
                .Bind<IWebRequestService>()
                .To<WebRequestService>()
                .AsSingle();
        }

        private void BindWebRequestBuilder()
        {
            Container
                .Bind<IWebRequestBuilder>()
                .To<WebRequestBuilder>()
                .AsSingle();
        }

        private void BindAuthorizationSetter()
        {
            Container
                .Bind<IAuthorizationSetter>()
                .To<AuthorizationSetter>()
                .AsSingle();
        }

        private void BindWebRequestSenderFactory()
        {
            Container
                .Bind<IWebRequestSenderFactory>()
                .To<WebRequestSenderFactory>()
                .AsSingle();
        }

        private void BindWebRequest()
        {
            Container
                .Bind<IWebRequest>()
                .To<WebRequest>()
                .AsSingle();
        }

        private void BindTokenProvider()
        {
            Container
                .Bind<ITokenProvider>()
                .To<TokenProvider>()
                .AsSingle();
        }

        private void BindPersonalLogger()
        {
            Container
                .Bind<IPersonalLogger>()
                .To<PersonalLogger>()
                .AsSingle();
        }

        private void BindCoroutineRunner()
        {
            Container
                .Bind<ICoroutineRunner>()
                .FromInstance(_runner)
                .AsSingle();
        }

        private void BindConfigProvider()
        {
            Container
                .Bind<IConfigProvider>()
                .FromInstance(_configProvider)
                .AsSingle();
            
            _configProvider.BindConfigs(Container);
        }
    }
}