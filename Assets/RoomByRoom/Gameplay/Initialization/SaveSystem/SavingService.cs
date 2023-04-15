namespace RoomByRoom
{
	public class SavingService
	{
		private ISaver _saver;
		private string _profile;

		public SavingService(string profile, bool saveInFile)
		{
			_saver = saveInFile ? new Serializator() : new Database.DBAccessor();
			_profile = profile;
		}

		public bool LoadData(ref Saving saving) => _saver.LoadData(_profile, ref saving);

		public void SaveData(Saving saving) => _saver.SaveData(_profile, saving);
	}
}