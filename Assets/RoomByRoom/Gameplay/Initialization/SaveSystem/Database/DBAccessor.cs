using System.Collections.Generic;
using System.Data.SQLite;
using PlasticPipe.PlasticProtocol.Messages;
using RoomByRoom.Utility;

namespace RoomByRoom.Database
{
	public class DBAccessor
	{
		public readonly SQLiteCommand Command;
		private readonly string _dbFileName = Idents.FilePaths.DatabaseFileName;

		public DBAccessor()
		{
			var conn = new SQLiteConnection("Data Source = " + _dbFileName);

			conn.Open();
			Command = new SQLiteCommand
			{
				Connection = conn,
				// turn on foreign references
				CommandText = "PRAGMA foreign_keys=ON"
			};
			Command.ExecuteNonQuery();
		}

		~DBAccessor()
		{
			Command.Connection.Close();
		}
	}
}