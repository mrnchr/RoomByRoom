using Server.Database;
using Server.Infrastructure;

namespace Server.Profile
{
    public interface IProfileDataProvider : IDataProvider
    {
        public ProfileTable GetProfile(string name);
        public void UpdateProfile(ProfileTable profile);
        public void DeleteUser(string login);
    }
}