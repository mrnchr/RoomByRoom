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

        public bool LoadData(ref SavedData savedData) => _saver.LoadData(_profile, ref savedData);

        public void SaveData(SavedData savedData) => _saver.SaveData(_profile, savedData);
    }
}