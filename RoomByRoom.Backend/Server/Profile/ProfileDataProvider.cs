using Server.Database;

namespace Server.Profile
{
    public class ProfileDataProvider : IProfileDataProvider
    {
        private readonly ApplicationContext _ctx;

        public ProfileDataProvider(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        public ProfileTable GetProfile(string name)
        {
            return _ctx.Profiles.Single(x => x.User.Name == name);
        }

        public void UpdateProfile(ProfileTable profile)
        {
            _ctx.Update(profile);
            _ctx.SaveChanges();
        }

        public void DeleteUser(string login)
        {
            UserTable user = _ctx.Users.Single(x => x.Name == login);
            _ctx.Users.Remove(user);
            _ctx.SaveChanges();
        }
    }
}