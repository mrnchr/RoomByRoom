namespace RoomByRoom
{
    public interface ISaver
    {
        public bool LoadData(string fromFile, out SavedData savedData);
        public bool SaveData(string toFile, in SavedData savedData);
    }
}