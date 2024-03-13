using System.Threading.Tasks;

namespace RoomByRoom
{
  public class GameSaveService : IGameSaveService
  {
    private readonly ProgressData _progressData;
    private readonly ISaver _saver;
    private readonly ProgressData _defaultSave;

    public GameSaveService(ISaver saver, ProgressData progressData, ProgressDataSO defaultSave)
    {
      _saver = saver;
      _progressData = progressData;
      _defaultSave = defaultSave.Value;
    }
    
    public async Task LoadProgressAsync()
    {
      ProgressData progress = await _saver.LoadProgressAsync();
      if (progress == null)
        SaveDefault();
      else
        _progressData.Copy(progress);
    }

    public async void SaveProgressAsync()
    {
      await _saver.SaveProfileAsync(_progressData);
    }

    public void SaveDefault()
    {
      _progressData.Copy(_defaultSave);
      SaveProgressAsync();
    }
  }
}