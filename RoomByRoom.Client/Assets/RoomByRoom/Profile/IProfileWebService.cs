using System.Threading.Tasks;
using RoomByRoom.Web.Sender;
using SharedData.Profile;

namespace Profile
{
    public interface IProfileWebService
    {
        public Task<ProgressResponse> GetProgressAsync();
        Task<IWebRequestSender> UpdateProgressAsync(string progress);
        Task DeleteUserAsync();
    }
}