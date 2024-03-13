using System.Collections.Generic;
using RoomByRoom.Web.Building;
using RoomByRoom.Web.Sender;
using RoomByRoom.Web.Utils;

namespace RoomByRoom.Web.RequestService
{
    public class WebRequestService : IWebRequestService
    {
        private readonly IWebRequestSenderFactory _factory;
        private readonly IAuthorizationSetter _authSetter;

        public WebRequestService(IWebRequestSenderFactory factory, IAuthorizationSetter authSetter)
        {
            _factory = factory;
            _authSetter = authSetter;
        }
        
        public IWebRequestSender CreateRequest<TRequestData>(string method, TRequestData body,
            params string[] routes)
        {
            IWebRequestDataSetter setter = CreateSetter(method, body, routes);
            return _factory.Create(setter.GetData());
        }
        
        public IWebRequestSender CreateRequest(string method, params string[] routes)
        {
            IWebRequestDataSetter setter = CreateSetter(method, routes);
            return _factory.Create(setter.GetData());
        }

        private IWebRequestDataSetter CreateSetter<TRequestData>(string method, TRequestData body,
            IEnumerable<string> routes)
        {
            return CreateSetter(method, routes).WithBody(body);
        }

        private IWebRequestDataSetter CreateSetter(string method, IEnumerable<string> routes)
        {
            IWebRequestDataSetter setter = new WebRequestDataSetter()
                .SetConnection(method, new UriBuilder(routes).Build());
            
            if (_authSetter.IsAuthorized())
                setter.AsAuthorized();

            return setter;
        }
    }
}