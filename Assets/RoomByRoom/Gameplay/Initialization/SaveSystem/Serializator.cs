using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class Serializator : ISaver
	{
		private BinaryFormatter _formatter = new BinaryFormatter();

		public bool LoadData(string fromProfile, ref Saving saving)
		{
			try
			{
				using (FileStream fs = new FileStream(Idents.FilePaths.SavingDirectory + fromProfile, FileMode.Open))
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
			using FileStream fs = new FileStream(Idents.FilePaths.SavingDirectory + toProfile, FileMode.Create);
			_formatter.Serialize(fs, saving);
		}
	}
}