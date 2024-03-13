using System.Threading.Tasks;
using RoomByRoom.Web.RequestService;
using RoomByRoom.Web.Sender;
using RoomByRoom.Web.WebService;
using SharedData.Profile;

namespace Profile
{
    public class ProfileWebService : IProfileWebService
    {
        private readonly IWebService _web;

        public ProfileWebService(IWebService web)
        {
            _web = web;
        }

        public async Task<ProgressResponse> GetProgressAsync()
        {
            return await _web.SendAsync<ProgressResponse>(WebVerbs.GET, "profile", "progress");
        }

        public async Task<IWebRequestSender> UpdateProgressAsync(string progress)
        {
            var request = new ProgressRequest
            {
                Progress = progress
            };

            return await _web.SendAsync(WebVerbs.PUT, request, "profile", "progress");
        }

        public async Task DeleteUserAsync()
        {
            await _web.SendAsync(WebVerbs.DELETE, "profile");
        }
    }
}