using UnityEngine.Networking;

namespace RoomByRoom.Web.Building
{
    public interface IWebRequestBuilder
    {
        public UnityWebRequest Build(WebRequestData data);
    }
}