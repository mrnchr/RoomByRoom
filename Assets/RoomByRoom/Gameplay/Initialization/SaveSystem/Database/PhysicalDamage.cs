using System.Data.SQLite;

namespace RoomByRoom.Database
{
	public class PhysicalDamageTable : ITable<BoundComponent<ItemPhysicalDamage>>
	{
		public string GetTableName() => "phys_damage";

		public BoundComponent<ItemPhysicalDamage> Pull(SQLiteDataReader row)
		{
			var comp = new BoundComponent<ItemPhysicalDamage>();
			comp.BoundEntity = row.GetInt32(0);
			comp.ComponentInfo.Point = row.GetFloat(2);
			return comp;
		}

		public string GetTextToPut(BoundComponent<ItemPhysicalDamage> comp, string toProfile) =>
			"insert or replace into phys_damage values " +
			"(" +
			$"{comp.BoundEntity}, " +
			$"\'{toProfile}\', " +
			$"{comp.ComponentInfo.Point}" +
			");";
	}
}