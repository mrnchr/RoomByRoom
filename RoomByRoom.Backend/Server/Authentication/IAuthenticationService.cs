using SharedData.Authentication;

namespace Server.Authentication
{
    public interface IAuthenticationService
    {
        public (bool success, AuthenticationErrorType error) Register(string login, string password);
        public AuthenticationResponse Login(string login, string password);
    }
}