using System.Data.SQLite;

namespace RoomByRoom.Database
{
	public class ItemTable : ITable<BoundComponent<ItemInfo>>
	{
		public string GetTableName() => "item";

		public BoundComponent<ItemInfo> Pull(SQLiteDataReader row)
		{
			var comp = new BoundComponent<ItemInfo>
			{
				BoundEntity = row.GetInt32(0)
			};
			comp.ComponentInfo.Type = (ItemType)row.GetInt32(2);
			return comp;
		}

		public string GetTextToPut(BoundComponent<ItemInfo> comp, string toProfile) =>
			"insert or replace into item values " +
			"(" +
			$"{comp.BoundEntity}, " +
			$"\'{toProfile}\', " +
			$"{(int)comp.ComponentInfo.Type}" +
			");";
	}
}