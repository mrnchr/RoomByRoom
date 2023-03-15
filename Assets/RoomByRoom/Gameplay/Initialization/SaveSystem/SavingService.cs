using System.Runtime.Serialization.Formatters.Binary;

namespace RoomByRoom
{
    public class SavingService
    {
        private ISaver _saver;
        private string _saveName;

        public SavingService(string saveName)
        {
            _saver = new Serializator();
            _saveName = saveName;
        }

        public bool SaveData(SavedData savedData)
        {
            return _saver.SaveData(_saveName, savedData);
        }

        public bool LoadData(out SavedData savedData)
        {
            return _saver.LoadData(_saveName, out savedData);
        }
    }
}