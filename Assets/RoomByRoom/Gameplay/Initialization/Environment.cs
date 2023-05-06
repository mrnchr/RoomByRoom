using System.IO;
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
        var db = new DBAccessor();
        db.Command.CommandText = File.ReadAllText(Application.dataPath + "/Dump/room_by_room.sql");
        db.Command.ExecuteNonQuery();
      }
    }
  }
}