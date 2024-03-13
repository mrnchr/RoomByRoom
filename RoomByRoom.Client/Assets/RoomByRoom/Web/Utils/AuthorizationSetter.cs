using UnityEngine.Networking;

namespace RoomByRoom.Web.Utils
{
    public class AuthorizationSetter : IAuthorizationSetter
    {
        private readonly ITokenProvider _provider;

        public AuthorizationSetter(ITokenProvider provider)
        {
            _provider = provider;
        }

        public bool IsAuthorized()
        {
            return !string.IsNullOrEmpty(_provider.Token);
        }
        
        public void SetAuthorization(UnityWebRequest www)
        {
            www.SetRequestHeader("Authorization", "Bearer " + _provider.Token);
        }
    }
}