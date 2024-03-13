using System;
using UnityEngine.Networking;

namespace RoomByRoom.Web.Building
{
    public interface IWebRequestDataSetter
    {
        public IWebRequestDataSetter SetConnection(string method, string localUrl);
        public IWebRequestDataSetter WithBody<TRequestData>(TRequestData data);
        public IWebRequestDataSetter AsAuthorized();
        public IWebRequestDataSetter Configure(Action<UnityWebRequest> configurator);
        public WebRequestData GetData();
    }
}