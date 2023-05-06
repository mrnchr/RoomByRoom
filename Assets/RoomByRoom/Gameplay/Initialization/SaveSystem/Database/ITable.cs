using System.Data.SQLite;

namespace RoomByRoom.Database
{
  public interface ITable<T> where T : struct
  {
    public string GetTableName();
    public T Pull(SQLiteDataReader row);
    public string GetTextToPut(T comp, string toProfile);
  }
}