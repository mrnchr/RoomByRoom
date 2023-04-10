using System.Data.SQLite;

namespace RoomByRoom.Database
{
	public class ProtectionTable : ITable<BoundComponent<ItemPhysicalProtection>>
	{
		public string GetTableName() => "protection";

		public BoundComponent<ItemPhysicalProtection> Pull(SQLiteDataReader row)
		{
			BoundComponent<ItemPhysicalProtection> comp = new BoundComponent<ItemPhysicalProtection>();
			comp.BoundEntity = row.GetInt32(0);
			comp.ComponentInfo.Point = row.GetFloat(2);
			return comp;
		}

		public string GetTextToPut(BoundComponent<ItemPhysicalProtection> comp, string toProfile)
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