using UnityEngine.Networking;

namespace RoomByRoom.Web.Low
{
    public interface IWebRequest
    {
        public UnityWebRequest Create(string method, string localUrl, string requestData);
        public UnityWebRequest Create(string method, string localUrl);
    }
}