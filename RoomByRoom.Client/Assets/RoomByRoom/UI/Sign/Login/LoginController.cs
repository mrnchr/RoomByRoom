using Authorization;

namespace UI.Menu
{
    public class LoginController : ILoginController
    {
        private readonly IAuthorizationProcessor _processor;
        private readonly AuthorizationModel _model;

        public LoginController(IAuthorizationProcessor processor, AuthorizationModel model)
        {
            _processor = processor;
            _model = model;
        }

        public void Login(string login, string password)
        {
            if (_model.IsWaiting)
                return;
            
            _model.Name = login;
            _model.Password = password;
            _processor.Login();
        }
    }
}