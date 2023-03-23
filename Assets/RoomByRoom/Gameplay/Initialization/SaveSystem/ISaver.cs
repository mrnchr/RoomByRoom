namespace RoomByRoom
{
    public interface ISaver
    {
        public bool LoadData(string fromProfile, ref SavedData savedData);
        public bool SaveData(string toProfile, SavedData savedData);
    }
}