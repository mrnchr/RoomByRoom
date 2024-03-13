using System;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace RoomByRoom.Web.Building
{
    public class WebRequestDataSetter : IWebRequestDataSetter
    {
        private readonly WebRequestData _data;

        public WebRequestDataSetter()
        {
            _data = new WebRequestData();
        }
        
        public WebRequestDataSetter(WebRequestData data)
        {
            _data = data;
        }

        public IWebRequestDataSetter SetConnection(string method, string localUrl)
        {
            _data.Method = method;
            _data.LocalUrl = localUrl;

            return this;
        }

        public IWebRequestDataSetter WithBody<TRequestData>(TRequestData data)
        {
            _data.HasData = true;
            _data.Data = JsonConvert.SerializeObject(data);
            return this;
        }

        public IWebRequestDataSetter AsAuthorized()
        {
            _data.IsAuthorized = true;
            return this;
        }

        public IWebRequestDataSetter Configure(Action<UnityWebRequest> configurator)
        {
            _data.Configurator = configurator;
            return this;
        }

        public WebRequestData GetData()
        {
            return _data;
        }
    }
}