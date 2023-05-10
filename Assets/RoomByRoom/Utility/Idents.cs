namespace RoomByRoom.Utility
{
  public static class Idents
  {
    public static class Worlds
    {
      public const string MessageWorld = "ForMessages";
    }

    public static class FilePaths
    {
      public const string SavingDirectory =
#if UNITY_EDITOR
        @"Local\Saves\";
#elif UNITY_STANDALONE || DEVELOPMENT_BUILD
				@".local\Saves\";
#endif
      
      public const string DatabaseFileName =
#if UNITY_EDITOR
        @"Local\room_by_room.db";
#elif UNITY_STANDALONE || DEVELOPMENT_BUILD
				@".local\room_by_room.db";
#endif
      
      public const string ConfigFileName =
#if UNITY_EDITOR
        @"Local\config.xml";
#elif UNITY_STANDALONE || DEVELOPMENT_BUILD
				@".local\config.xml";
#endif
    }
  }
}