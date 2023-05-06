using System.Data.SQLite;

namespace RoomByRoom.Database
{
  public class PhysProtectionTable : ITable<BoundComponent<ItemPhysicalProtection>>
  {
    public string GetTableName() => "phys_protection";

    public BoundComponent<ItemPhysicalProtection> Pull(SQLiteDataReader row)
    {
      var comp = new BoundComponent<ItemPhysicalProtection>();
      comp.BoundEntity = row.GetInt32(0);
      comp.ComponentInfo.Point = row.GetFloat(2);
      return comp;
    }

    public string GetTextToPut(BoundComponent<ItemPhysicalProtection> comp, string toProfile) =>
      $"insert or replace into {GetTableName()} values " +
      "(" +
      $"{comp.BoundEntity}, " +
      $"\'{toProfile}\', " +
      $"{comp.ComponentInfo.Point}" +
      ");";
  }
}