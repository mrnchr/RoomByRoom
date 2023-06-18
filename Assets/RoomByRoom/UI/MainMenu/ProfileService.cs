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
    [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable", Justification = "Destructor closes connection")] 
    private readonly DbAccessor _db;
    

    public ProfileService()
    {
      _db = new DbAccessor();
    }

    public string[] Load() => LoadFromDB();

    private string[] LoadFromDB()
    {
      using var db = _db.GetConnection();
      return (from p in db.TProfile
        select p.Name)
        .ToArray();
    }

    public void Delete(string profileName) => RemoveFromDB(profileName);

    private void RemoveFromDB(string profileName)
    {
      using var db = _db.GetConnection();
      db.TProfile
        .Where(x => x.Name == profileName)
        .Delete();
    }
  }
}