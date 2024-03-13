using SharedData.Authentication;

namespace Authorization
{
    public class AuthorizationModel
    {
        public readonly ChangedProperty<AuthenticationErrorType> AuthError = new ChangedProperty<AuthenticationErrorType>();
        public readonly ChangedProperty<InputErrorType> InputError = new ChangedProperty<InputErrorType>();
        public readonly ChangedProperty<bool> IsWaiting = new ChangedProperty<bool>();
        public string Name;
        public string Password;
        public string ConfirmedPassword;
    }
}