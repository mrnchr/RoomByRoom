namespace RoomByRoom
{
  public interface ISaver
  {
    public bool ProfileExists(string profile);
    public GameSave LoadProfile(string profile);
    public void SaveProfile(string profile, GameSave gameSave);
  }
}