using System.Data.SQLite;

namespace RoomByRoom.Database
{
    public class PhysicalDamageTable : ITable<BoundComponent<PhysicalDamage>>
    {
        public string GetTableName() => "phys_damage";

        public BoundComponent<PhysicalDamage> Pull(SQLiteDataReader row)
        {
            BoundComponent<PhysicalDamage> comp = new BoundComponent<PhysicalDamage>();
            comp.BoundEntity = row.GetInt32(0);
            comp.ComponentInfo.Point = row.GetFloat(2);
            return comp;
        }

        public string GetTextToPut(BoundComponent<PhysicalDamage> comp, string toProfile)
        {
            return $"insert or replace into phys_damage values " +
                $"(" +
                $"{comp.BoundEntity}, " +
                $"\'{toProfile}\', " +
                $"{comp.ComponentInfo.Point}" +
                $");";
        }
    }
}