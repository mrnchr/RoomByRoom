using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class Serializator : ISaver
	{
		private readonly BinaryFormatter _formatter = new BinaryFormatter();

		public bool LoadData(string fromProfile, ref Saving saving)
		{
			try
			{
				using (var fs = new FileStream(Idents.FilePaths.SavingDirectory + fromProfile, FileMode.Open))
				{
					saving = (Saving)_formatter.Deserialize(fs);
				}

				return true;
			}
			catch (IOException)
			{
				return false;
			}
		}

		public void SaveData(string toProfile, Saving saving)
		{
			using var fs = new FileStream(Idents.FilePaths.SavingDirectory + toProfile, FileMode.Create);
			_formatter.Serialize(fs, saving);
		}
	}
}