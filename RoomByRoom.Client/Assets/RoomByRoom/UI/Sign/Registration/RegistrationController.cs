using Authorization;

namespace UI.Menu.Registration
{
    public class RegistrationController : IRegistrationController
    {
        private readonly IAuthorizationProcessor _processor;
        private readonly AuthorizationModel _model;

        public RegistrationController(IAuthorizationProcessor processor, AuthorizationModel model)
        {
            _processor = processor;
            _model = model;
        }

        public void Register(string login, string password, string confirmedPassword)
        {
            if (_model.IsWaiting)
                return;
            
            _model.Name = login;
            _model.Password = password;
            _model.ConfirmedPassword = confirmedPassword;
            _processor.Register();
        }
    }
}