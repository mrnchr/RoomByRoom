using Infrastructure.Logging;
using RoomByRoom.Web.Building;

namespace RoomByRoom.Web.Sender
{
    public class WebRequestSenderFactory : IWebRequestSenderFactory
    {
        private readonly IWebRequestBuilder _builder;
        private readonly IPersonalLogger _logger;

        public WebRequestSenderFactory(IWebRequestBuilder builder, IPersonalLogger logger)
        {
            _builder = builder;
            _logger = logger;
        }

        public IWebRequestSender Create(WebRequestData data)
        {
            return new WebRequestSender(data, _builder, _logger);
        }
    }
}