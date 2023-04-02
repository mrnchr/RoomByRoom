using System.Data.SQLite;

namespace RoomByRoom.Database
{
    public class WeaponTable : ITable<BoundComponent<WeaponInfo>>
    {
        public string GetTableName() => "weapon";

        public BoundComponent<WeaponInfo> Pull(SQLiteDataReader row)
        {
            BoundComponent<WeaponInfo> comp = new BoundComponent<WeaponInfo>();
            comp.BoundEntity = row.GetInt32(0);
            comp.ComponentInfo.Type = (WeaponType)row.GetInt32(2);
            return comp;
        }

        public string GetTextToPut(BoundComponent<WeaponInfo> comp, string toProfile)
        {
            return $"insert or replace into weapon values " +
                $"(" +
                $"{comp.BoundEntity}, " +
                $"\'{toProfile}\', " +
                $"{(int)comp.ComponentInfo.Type}" +
                $");";
        }
    }
}