using System.Data.SQLite;

namespace RoomByRoom.Database
{
	public class ArmorTable : ITable<BoundComponent<ArmorInfo>>
	{
		public string GetTableName() => "armor";

		public BoundComponent<ArmorInfo> Pull(SQLiteDataReader row)
		{
			BoundComponent<ArmorInfo> comp = new BoundComponent<ArmorInfo>
			{
				BoundEntity = row.GetInt32(0)
			};
			comp.ComponentInfo.Type = (ArmorType)row.GetInt32(2);
			return comp;
		}

		public string GetTextToPut(BoundComponent<ArmorInfo> comp, string toProfile) =>
			"insert or replace into weapon values " +
			"(" +
			$"{comp.BoundEntity}, " +
			$"\'{toProfile}\', " +
			$"{(int)comp.ComponentInfo.Type}" +
			");";
	}
}