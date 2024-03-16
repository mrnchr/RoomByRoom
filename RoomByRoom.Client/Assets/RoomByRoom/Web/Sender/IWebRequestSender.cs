using System.Collections;
using System.Threading.Tasks;

namespace RoomByRoom.Web.Sender
{
    public interface IWebRequestSender
    {
        public RequestSnapshot Snapshot { get; }
        
        TResponseData GetData<TResponseData>();
        public bool IsConnectionError();
        public bool IsSuccess();
        IEnumerator SendRequest();
        public Task SendRequestAsync();
    }
}