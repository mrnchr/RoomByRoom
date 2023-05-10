namespace RoomByRoom.UI.MainMenu
{
  public interface IFactory<out T>
  {
    public T Create();
  }
}