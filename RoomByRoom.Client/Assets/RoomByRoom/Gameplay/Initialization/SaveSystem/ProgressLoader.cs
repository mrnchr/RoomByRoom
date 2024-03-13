using Zenject;

namespace RoomByRoom.Infrastructure.Initializing
{
  public class ProgressLoader : IInitializable, IProgressLoader
  {
    private readonly IGameSaveService _saveSvc;

    public bool IsLoaded { get; private set; }

    public ProgressLoader(IGameSaveService saveSvc)
    {
      _saveSvc = saveSvc;
    }
    
    public void Initialize()
    {
      InitializeAsync();
    }

    private async void InitializeAsync()
    {
      await _saveSvc.LoadProgressAsync();
      IsLoaded = true;
    }
  }
}