using System.Threading.Tasks;
using RoomByRoom.Web.Sender;
using UnityEngine;

namespace RoomByRoom.Web.Processor
{
    public interface IWebRequestProcessor
    {
        public Coroutine Process(IWebRequestSender sender);
        public Task ProcessAsync(IWebRequestSender sender);
    }
}