using System.Threading.Tasks;

namespace RoomByRoom
{
  public interface ISaver
  {
    public Task<ProgressData> LoadProgressAsync();
    public Task SaveProfileAsync(ProgressData progressData);
  }
}