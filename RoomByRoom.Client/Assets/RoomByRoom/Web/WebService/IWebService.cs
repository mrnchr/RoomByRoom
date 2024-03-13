using System.Threading.Tasks;
using RoomByRoom.Web.Sender;

namespace RoomByRoom.Web.WebService
{
    public interface IWebService
    {
        public Task<TResponse> SendAsync<TRequest, TResponse>(string method, TRequest request, params string[] routes);
        public Task<TResponse> SendAsync<TResponse>(string method, params string[] routes);
        public Task<IWebRequestSender> SendAsync<TRequest>(string method, TRequest request, params string[] routes);
        public Task<IWebRequestSender> SendAsync(string method, params string[] routes);
    }
}