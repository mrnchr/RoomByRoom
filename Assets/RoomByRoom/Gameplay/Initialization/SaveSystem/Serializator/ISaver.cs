namespace RoomByRoom
{
  public interface ISaver
  {
    public bool LoadData(string profile, ref Saving saving);
    public void SaveData(string profile, Saving saving);
  }
}