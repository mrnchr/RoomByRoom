using RoomByRoom.Database;

namespace RoomByRoom
{
  public class SavingService
  {
    public readonly string ProfileName;
    private readonly ISaver _saver;

    public SavingService(string profileName, bool saveInFile)
    {
      _saver = saveInFile ? new Serializator() : new DBGameSaver();
      ProfileName = profileName;
    }

    public bool LoadData(ref Saving saving) => _saver.LoadData(ProfileName, ref saving);

    public void SaveData(Saving saving)
    {
      _saver.SaveData(ProfileName, saving);
    }
  }
}