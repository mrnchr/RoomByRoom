namespace RoomByRoom
{
    public class SavingService
    {
        private ISaver _saver;
        private string _profileName;

        public SavingService(string profileName, bool saveInFile)
        {
            _saver = saveInFile ? new Serializator() : new DBAccessor();
            _profileName = profileName;
        }

        public bool LoadData(ref SavedData savedData)
        {
            return _saver.LoadData(_profileName, ref savedData);
        }
        
        public bool SaveData(SavedData savedData)
        {
            return _saver.SaveData(_profileName, savedData);
        }
    }
}