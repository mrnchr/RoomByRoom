using System.IO;
using LinqToDB.Data;
using RoomByRoom.Database;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom.Initialization
{
  public static class Environment
  {
    public static void Prepare(bool saveInFile)
    {
      if (saveInFile)
      {
        if(!Directory.Exists(Idents.FilePaths.SavingDirectory))
          Directory.CreateDirectory(Idents.FilePaths.SavingDirectory);
      }
      else
      {
        string dbFilePath = Idents.FilePaths.DatabaseFileName;
        if (File.Exists(dbFilePath)) return;
        Directory.CreateDirectory(dbFilePath[..dbFilePath.LastIndexOf('\\')]);
        using var db = new DbAccessor().GetConnection();
        db.Execute(File.ReadAllText(Application.streamingAssetsPath + "/room_by_room.sql"));
      }
    }
  }
}