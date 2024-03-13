using RoomByRoom.Web.Low;
using RoomByRoom.Web.Utils;
using UnityEngine.Networking;

namespace RoomByRoom.Web.Building
{
    public class WebRequestBuilder : IWebRequestBuilder
    {
        private readonly IWebRequest _web;
        private readonly IAuthorizationSetter _authSvc;

        public WebRequestBuilder(IWebRequest web, IAuthorizationSetter authSvc)
        {
            _web = web;
            _authSvc = authSvc;
        }

        public UnityWebRequest Build(WebRequestData data)
        {
            UnityWebRequest request = data.HasData 
                ? _web.Create(data.Method, data.LocalUrl, data.Data) 
                : _web.Create(data.Method, data.LocalUrl);
            
            if (data.IsAuthorized && _authSvc.IsAuthorized())
                _authSvc.SetAuthorization(request);
            
            data.Configurator?.Invoke(request);

            return request;
        }
    }
}