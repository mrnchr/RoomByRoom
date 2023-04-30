using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using RoomByRoom.Database;
using RoomByRoom.Utility;

namespace RoomByRoom.UI.MainMenu
{
	public class ProfileService
	{
		private readonly bool _saveInFile;
		private readonly SQLiteCommand _comm;

		public ProfileService(bool saveInFile)
		{
			_saveInFile = saveInFile;
			_comm = new DBAccessor().Command;
		}

		public string[] Load()
		{
			return _saveInFile
				? LoadFromFile()
				: LoadFromDB();
		}

		private string[] LoadFromDB()
		{
			_comm.CommandText = "SELECT name FROM profile";
			SQLiteDataReader reader = _comm.ExecuteReader();
			var profileNames = new List<string>();

			while(reader.Read())
				profileNames.Add(reader.GetString(0));

			return profileNames.ToArray();
		}

		private string[] LoadFromFile()
		{
			var profileFolder = new DirectoryInfo(Idents.FilePaths.SavingDirectory);
			return profileFolder.GetFiles()
#if UNITY_EDITOR
				.Where(x => !x.Name.EndsWith(".meta"))
#endif
				.OrderByDescending(x => x.LastWriteTime)
				.Select(x => x.Name)
				.ToArray();
		}
	}
}