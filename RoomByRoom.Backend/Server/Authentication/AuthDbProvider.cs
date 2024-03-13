using Server.Database;

namespace Server.Authentication
{
    public class AuthDbProvider : IAuthDataProvider
    {
        private readonly ApplicationContext _ctx;

        public AuthDbProvider(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        public bool HasUserByLogin(string name)
        {
            return _ctx.Users.Any(x => x.Name == name);
        }

        public void AddUser(UserTable user)
        {
            _ctx.Users.Add(user);
            _ctx.SaveChanges();
            
            user = _ctx.Users.Single(x => x.Name == user.Name);
            _ctx.Profiles.Add(new ProfileTable
            {
                UserId = user.Id,
                Progress = null
            });
            _ctx.SaveChanges();
        }

        public UserTable? GetUser(string name)
        {
            return _ctx.Users.SingleOrDefault(x => x.Name == name);
        }
    }
}