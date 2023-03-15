using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class Serializator : ISaver
    {
        private BinaryFormatter _formatter = new BinaryFormatter();

        public bool LoadData(string fromFile, out SavedData savedData)
        {
            savedData = new SavedData();

            try
            {
                using (FileStream fs = new FileStream(Idents.FilePaths.SavingDirectory + fromFile, FileMode.Open))
                {
                    savedData = (SavedData)_formatter.Deserialize(fs);
                }

                return true;
            }
            catch(IOException)
            {
                return false;
            }
        }

        public bool SaveData(string toFile, in SavedData savedData)
        {
            using (FileStream fs = new FileStream(Idents.FilePaths.SavingDirectory + toFile, FileMode.Create))
            {
                _formatter.Serialize(fs, savedData);
            }

            return true;
        }
    }
}