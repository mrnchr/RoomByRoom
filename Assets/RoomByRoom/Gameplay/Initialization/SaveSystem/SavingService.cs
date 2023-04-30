using RoomByRoom.Database;

namespace RoomByRoom
{
	public class SavingService
	{
		private readonly string _profile;
		private readonly ISaver _saver;

		public SavingService(string profile, bool saveInFile)
		{
			_saver = saveInFile ? new Serializator() : new DBGameSaver();
			_profile = profile;
		}

		public bool LoadData(ref Saving saving) => _saver.LoadData(_profile, ref saving);

		public void SaveData(Saving saving)
		{
			_saver.SaveData(_profile, saving);
		}
	}
}