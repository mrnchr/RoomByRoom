using System.Threading.Tasks;
using Infrastructure.Logging;
using Newtonsoft.Json;
using Profile;
using RoomByRoom;
using SharedData.Profile;

namespace UI.Profile
{
    public class ProgressSaver : ISaver
    {
        private readonly IProfileWebService _webSvc;
        private readonly IPersonalLogger _logger;

        public ProgressSaver(IProfileWebService webSvc, IPersonalLogger logger)
        {
            _webSvc = webSvc;
            _logger = logger;
        }

        public async Task<ProgressData> LoadProgressAsync()
        {
            ProgressResponse progress = await _webSvc.GetProgressAsync();

            _logger.Log($"Progress for user was loaded", LoggingGroups.PROFILE);
            return progress.Progress != ""
                ? JsonConvert.DeserializeObject<ProgressData>(progress.Progress)
                : null;
        }

        public async Task SaveProfileAsync(ProgressData progressData)
        {
            await _webSvc.UpdateProgressAsync(JsonConvert.SerializeObject(progressData));
        }
    }
}