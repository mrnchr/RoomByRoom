using LinqToDB;
using RoomByRoom.Utility;

namespace RoomByRoom.Database
{
  public class DbAccessor
  {
    private readonly DataOptions _dataOptions;
    private const string _dbFileName = Idents.FilePaths.DatabaseFileName;

    public DbAccessor() =>
      _dataOptions = new DataOptions()
        .UseSQLite($"Data Source = {_dbFileName}; Foreign Keys = True");

    public DbRoomByRoomConnection GetConnection() => new DbRoomByRoomConnection(_dataOptions);
  }
}