using System.Data.SQLite;

namespace RoomByRoom.Database
{
    public class ProtectionTable : ITable<BoundComponent<Protection>>
    {
        public string GetTableName() => "protection";

        public BoundComponent<Protection> Pull(SQLiteDataReader row)
        {
            BoundComponent<Protection> comp = new BoundComponent<Protection>();
            comp.BoundEntity = row.GetInt32(0);
            comp.ComponentInfo.Point = row.GetFloat(2);
            return comp;
        }

        public string GetTextToPut(BoundComponent<Protection> comp, string toProfile)
        {
            return $"insert or replace into protection values " +
                $"(" +
                $"{comp.BoundEntity}, " +
                $"\'{toProfile}\', " +
                $"{comp.ComponentInfo.Point}" +
                $");";
        }
    }
}