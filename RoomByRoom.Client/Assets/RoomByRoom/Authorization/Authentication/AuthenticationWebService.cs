using System.Threading.Tasks;
using RoomByRoom.Web.RequestService;
using RoomByRoom.Web.WebService;
using SharedData.Authentication;

namespace Authorization
{
    public class AuthenticationWebService : IAuthenticationWebService
    {
        private readonly IWebService _web;
        
        public AuthenticationWebService(IWebService web)
        {
            _web = web;
        }

        public async Task<AuthenticationResponse> RegisterAsync(string login, string password)
        {
            var user = new AuthenticationRequest
            {
                Login = login,
                Password = password
            };

           return await _web.SendAsync<AuthenticationRequest, AuthenticationResponse>(WebVerbs.POST, user, "authentication",
               "register");
        }

        public async Task<AuthenticationResponse> LoginAsync(string login, string password)
        {
            var user = new AuthenticationRequest
            {
                Login = login,
                Password = password
            };

            return await _web.SendAsync<AuthenticationRequest, AuthenticationResponse>(WebVerbs.POST, user, "authentication",
                "login");
        }

        public async Task LogoutAsync()
        {
            await _web.SendAsync(WebVerbs.DELETE, "authentication", "logout");
        }
    }
}