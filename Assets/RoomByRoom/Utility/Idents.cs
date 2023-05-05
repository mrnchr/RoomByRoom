namespace RoomByRoom.Utility
{
	public static class Idents
	{
		public static class Worlds
		{
			public static string MessageWorld = "ForMessages";
		}

		public static class FilePaths
		{
			public const string SavingDirectory =
#if UNITY_EDITOR
				@"Assets\_Local\Saves\";
#elif UNITY_STANDALONE || DEVELOPMENT_BUILD
				@"Local\Saves\";
#endif
			public const string DatabaseFileName =
#if UNITY_EDITOR
				@"Assets\_Local\room_by_room.db";
#elif UNITY_STANDALONE || DEVELOPMENT_BUILD
				@"Local\room_by_room.db";
#endif
		}
	}
}