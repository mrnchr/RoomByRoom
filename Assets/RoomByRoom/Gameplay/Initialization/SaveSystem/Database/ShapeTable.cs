using System.Data.SQLite;

namespace RoomByRoom.Database
{
  public class ShapeTable : ITable<BoundComponent<Shape>>
  {
    public string GetTableName() => "shape";

    public BoundComponent<Shape> Pull(SQLiteDataReader row)
    {
      var comp = new BoundComponent<Shape>();
      comp.BoundEntity = row.GetInt32(0);
      comp.ComponentInfo.PrefabIndex = row.GetInt32(2);
      return comp;
    }

    public string GetTextToPut(BoundComponent<Shape> comp, string toProfile) =>
      "insert or replace into shape values " +
      "(" +
      $"{comp.BoundEntity}, " +
      $"\'{toProfile}\', " +
      $"{comp.ComponentInfo.PrefabIndex}" +
      ");";
  }
}