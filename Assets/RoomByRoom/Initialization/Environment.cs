using System.IO;
using LinqToDB.Data;
using RoomByRoom.Control;
using RoomByRoom.Database;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom.Initialization
{
  public static class Environment
  {
    public static void LoadLocalFiles(ConfigurationSO defaultCfg)
    {
      LoadConfig(defaultCfg);
      LoadDb();
    }

    private static void LoadConfig(ConfigurationSO defaultCfg)
    {
      if (File.Exists(Idents.FilePaths.ConfigFileName)) return;
      
      new ConfigSaveService(Idents.FilePaths.ConfigFileName)
        .Save(defaultCfg.Value);
    }

    private static void LoadDb()
    {
      string dbFilePath = Idents.FilePaths.DatabaseFileName;
      if (File.Exists(dbFilePath)) return;

      Directory.CreateDirectory(dbFilePath[..dbFilePath.LastIndexOf('\\')]);
      using var db = new DbAccessor().GetConnection();
      db.Execute(File.ReadAllText(Application.streamingAssetsPath + "/room_by_room.sql"));
    }
  }
}