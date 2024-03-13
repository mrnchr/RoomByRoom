using System.Threading.Tasks;
using SharedData.Authentication;

namespace Authorization
{
    public interface IAuthenticationWebService
    {
        public Task<AuthenticationResponse> RegisterAsync(string login, string password);
        public Task<AuthenticationResponse> LoginAsync(string login, string password);
        Task LogoutAsync();
    }
}