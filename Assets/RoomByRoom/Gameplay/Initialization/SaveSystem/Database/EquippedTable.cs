using System.Data.SQLite;

namespace RoomByRoom.Database
{
    public class EquippedTable : ITable<BoundComponent<Equipped>>
    {
        public string GetTableName() => "equipped";

        public BoundComponent<Equipped> Pull(SQLiteDataReader row)
        {
            BoundComponent<Equipped> comp = new BoundComponent<Equipped>();
            comp.BoundEntity = row.GetInt32(0);
            return comp;
        }

        public string GetTextToPut(BoundComponent<Equipped> comp, string toProfile)
        {
            return $"insert or replace into equipped values " +
                $"(" +
                $"{comp.BoundEntity}, " +
                $"\'{toProfile}\' " +
                $");";
        }
    }
}