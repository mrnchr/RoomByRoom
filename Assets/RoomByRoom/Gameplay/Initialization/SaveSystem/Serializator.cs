using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class Serializator : ISaver
	{
		private BinaryFormatter _formatter = new BinaryFormatter();

		public bool LoadData(string fromProfile, ref SavedData savedData)
		{
			try
			{
				using (FileStream fs = new FileStream(Idents.FilePaths.SavingDirectory + fromProfile, FileMode.Open))
				{
					savedData = (SavedData)_formatter.Deserialize(fs);
				}

				return true;
			}
			catch (IOException)
			{
				return false;
			}
		}

		public void SaveData(string toProfile, SavedData savedData)
		{
			using FileStream fs = new FileStream(Idents.FilePaths.SavingDirectory + toProfile, FileMode.Create);
			_formatter.Serialize(fs, savedData);
		}
	}
}