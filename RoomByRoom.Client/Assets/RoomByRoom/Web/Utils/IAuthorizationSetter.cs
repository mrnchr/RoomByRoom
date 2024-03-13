using UnityEngine.Networking;

namespace RoomByRoom.Web.Utils
{
    public interface IAuthorizationSetter
    {
        public bool IsAuthorized();
        public void SetAuthorization(UnityWebRequest www);
    }
}