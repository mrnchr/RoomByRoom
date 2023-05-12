using System.IO;
using LinqToDB.Data;
using RoomByRoom.Database;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom.Initialization
{
  public static class Environment
  {
    public static void Prepare()
    {
      if (!Directory.Exists(Idents.FilePaths.SavingDirectory))
      {
        Directory.CreateDirectory(Idents.FilePaths.SavingDirectory);
        using var db = new DbAccessor().GetConnection();
        db.Execute(File.ReadAllText(Application.streamingAssetsPath + "/room_by_room.sql"));
      }
    }
  }
}