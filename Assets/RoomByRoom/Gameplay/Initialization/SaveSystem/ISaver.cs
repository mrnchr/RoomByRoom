namespace RoomByRoom
{
	public interface ISaver
	{
		public bool LoadData(string profile, ref SavedData savedData);
		public void SaveData(string profile, SavedData savedData);
	}
}