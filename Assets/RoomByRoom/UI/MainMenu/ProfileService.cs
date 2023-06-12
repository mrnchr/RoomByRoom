using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using LinqToDB;
using RoomByRoom.Database;
using RoomByRoom.Utility;

namespace RoomByRoom.UI.MainMenu
{
  public class ProfileService
  {
    private readonly bool _saveInFile;
    [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable", Justification = "Destructor closes connection")] 
    private readonly DbAccessor _db;
    

    public ProfileService(bool saveInFile)
    {
      _saveInFile = saveInFile;
      if (saveInFile) return;
      _db = new DbAccessor();
    }

    public string[] Load() =>
      _saveInFile
        ? LoadFromFile()
        : LoadFromDB();

    private string[] LoadFromDB()
    {
      using var db = _db.GetConnection();
      return (from p in db.TProfile
        select p.Name)
        .ToArray();
    }

    private static string[] LoadFromFile()
    {
      var profileFolder = new DirectoryInfo(Idents.FilePaths.SavingDirectory);
      return profileFolder.GetFiles()
#if UNITY_EDITOR
        .Where(x => !x.Name.EndsWith(".meta"))
#endif
        .OrderByDescending(x => x.LastWriteTime)
        .Select(x => x.Name)
        .ToArray();
    }

    public void Delete(string profileName)
    {
      if (_saveInFile)
        RemoveFile(profileName);
      else
        RemoveFromDB(profileName);
    }

    private static void RemoveFile(string profileName) =>
      File.Delete(Idents.FilePaths.SavingDirectory + profileName);

    private void RemoveFromDB(string profileName)
    {
      using var db = _db.GetConnection();
      db.TProfile
        .Where(x => x.Name == profileName)
        .Delete();
    }
  }
}