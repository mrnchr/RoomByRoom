using System.Threading.Tasks;
using RoomByRoom.Web.Processor;
using RoomByRoom.Web.RequestService;
using RoomByRoom.Web.Sender;

namespace RoomByRoom.Web.WebService
{
    public class WebService : IWebService
    {
        private readonly IWebRequestService _service;
        private readonly IWebRequestProcessor _processor;

        public WebService(IWebRequestService service, IWebRequestProcessor processor)
        {
            _service = service;
            _processor = processor;
        }
        
        public async Task<TResponse> SendAsync<TRequest, TResponse>(string method, TRequest request, params string[] routes)
        {
            IWebRequestSender sender = _service.CreateRequest(method, request, routes);
            await _processor.ProcessAsync(sender);
            
            return sender.GetData<TResponse>();
        }

        public async Task<TResponse> SendAsync<TResponse>(string method, params string[] routes)
        {
            IWebRequestSender sender = _service.CreateRequest(method, routes);
            await _processor.ProcessAsync(sender);
            
            return sender.GetData<TResponse>();
        }

        public async Task<IWebRequestSender> SendAsync<TRequest>(string method, TRequest request, params string[] routes)
        {
            IWebRequestSender sender = _service.CreateRequest(method, request, routes);
            await _processor.ProcessAsync(sender);
            
            return sender;
        }


        public async Task<IWebRequestSender> SendAsync(string method, params string[] routes)
        {
            IWebRequestSender sender = _service.CreateRequest(method, routes);
            await _processor.ProcessAsync(sender);
            
            return sender;
        }
    }
}