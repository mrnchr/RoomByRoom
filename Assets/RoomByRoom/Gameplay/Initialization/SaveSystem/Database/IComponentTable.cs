namespace RoomByRoom.Database
{
  public interface IComponentTable<T>
    where T : struct
  {
    public int Id { get; set; }
    public string ProfileName { get; set; }
    public T GetComponent();
    public void SetComponent(T comp, string profileName);
  }
}