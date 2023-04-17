using System.Data.SQLite;

namespace RoomByRoom.Database
{
	public class EquippedTable : ITable<BoundComponent<Equipped>>
	{
		public string GetTableName() => "equipped";

		public BoundComponent<Equipped> Pull(SQLiteDataReader row) =>
			new BoundComponent<Equipped>
			{
				BoundEntity = row.GetInt32(0)
			};

		public string GetTextToPut(BoundComponent<Equipped> comp, string toProfile) =>
			"insert or replace into equipped values " +
			"(" +
			$"{comp.BoundEntity}, " +
			$"\'{toProfile}\' " +
			");";
	}
}