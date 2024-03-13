using Infrastructure.Logging;
using Infrastructure.SceneLoading;
using RoomByRoom.Web.RequestService;
using RoomByRoom.Web.Sender;
using RoomByRoom.Web.Utils;
using RoomByRoom.Web.WebService;
using SharedData.Authentication;

namespace Authorization
{
    public class AuthorizationProcessor : IAuthorizationProcessor
    {
        private readonly IAuthenticationWebService _authSvc;
        private readonly AuthorizationModel _model;
        private readonly ICheckingService _checking;
        private readonly ITokenProvider _provider;
        private readonly IPersonalLogger _logger;
        private readonly IWebService _webSvc;
        private readonly ISceneLoadingService _sceneSvc;

        public AuthorizationProcessor(IAuthenticationWebService authSvc,
            AuthorizationModel model,
            ICheckingService checking,
            ITokenProvider provider,
            IPersonalLogger logger,
            IWebService webSvc,
            ISceneLoadingService sceneSvc)
        {
            _authSvc = authSvc;
            _model = model;
            _checking = checking;
            _provider = provider;
            _logger = logger;
            _webSvc = webSvc;
            _sceneSvc = sceneSvc;
        }

        public void Register()
        {
            if (_model.IsWaiting)
                return;
            
            RegisterAsync();
        }

        private async void RegisterAsync()
        {
            if (!_checking.Check())
                return;
            
            _model.IsWaiting.Value = true;
            
            AuthenticationResponse response = await _authSvc.RegisterAsync(_model.Name, _model.Password);
            ProcessResponse(response);

            _model.IsWaiting.Value = false;
        }

        public void Login()
        {
            if (_model.IsWaiting)
                return;
            
            LoginAsync();
        }

        private async void LoginAsync()
        {
            _model.IsWaiting.Value = true;
            
            AuthenticationResponse response = await _authSvc.LoginAsync(_model.Name, _model.Password);
            ProcessResponse(response);

            _model.IsWaiting.Value = false;
        }

        private void ProcessResponse(AuthenticationResponse response)
        {
            _model.AuthError.Value = response.Error;
            if (response.Error == AuthenticationErrorType.None)
            {
                _provider.Token = response.Token;

                CheckAuthorizationAsync();
                _sceneSvc.LoadMainScene();
            }
        }

        private async void CheckAuthorizationAsync()
        {
            IWebRequestSender sender = await _webSvc.SendAsync(WebVerbs.HEAD, "authorize");
            _logger.Log($"Authorization is {sender.Snapshot.Result}", LoggingGroups.WEB);
        }
    }
}