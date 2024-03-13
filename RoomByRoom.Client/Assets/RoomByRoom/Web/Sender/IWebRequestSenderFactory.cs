using RoomByRoom.Web.Building;

namespace RoomByRoom.Web.Sender
{
    public interface IWebRequestSenderFactory
    {
        public IWebRequestSender Create(WebRequestData data);
    }
}