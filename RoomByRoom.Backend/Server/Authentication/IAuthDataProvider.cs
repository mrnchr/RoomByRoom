using Server.Database;
using Server.Infrastructure;

namespace Server.Authentication
{
    public interface IAuthDataProvider : IDataProvider
    {
        public bool HasUserByLogin(string name);
        public void AddUser(UserTable user);
        public UserTable? GetUser(string name);
    }
}