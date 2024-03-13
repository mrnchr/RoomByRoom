using System.Threading.Tasks;

namespace RoomByRoom
{
  public interface IGameSaveService
  {
    public Task LoadProgressAsync();
    public void SaveProgressAsync();
    public void SaveDefault();
  }
}