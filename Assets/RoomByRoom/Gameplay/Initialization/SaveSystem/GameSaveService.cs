using RoomByRoom.Database;

namespace RoomByRoom
{
  public class GameSaveService
  {
    public readonly string ProfileName;
    private readonly ISaver _saver;
    private readonly GameSave _defaultSave;

    public GameSaveService(string profileName, GameSave defaultSave)
    {
      ProfileName = profileName;
      _saver = new DbGameSaver();
      _defaultSave = defaultSave;
    }
    
    public GameSave LoadProfile() => 
      _saver.ProfileExists(ProfileName) 
        ? _saver.LoadProfile(ProfileName) 
        : SaveDefault();

    public GameSave SaveDefault()
    {
      SaveProfile(_defaultSave);
      return _defaultSave;
    }

    public void SaveProfile(GameSave gameSave) => _saver.SaveProfile(ProfileName, gameSave);
  }
}