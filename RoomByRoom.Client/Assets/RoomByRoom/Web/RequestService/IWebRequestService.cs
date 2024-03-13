using RoomByRoom.Web.Sender;

namespace RoomByRoom.Web.RequestService
{
    public interface IWebRequestService
    {
        public IWebRequestSender CreateRequest<TRequestData>(string method, TRequestData body,
            params string[] routes);

        public IWebRequestSender CreateRequest(string method, params string[] routes);
    }
}