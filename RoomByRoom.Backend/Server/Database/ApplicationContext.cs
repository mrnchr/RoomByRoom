using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Server.Database
{
    public class ApplicationContext : DbContext
    {
        private readonly IWebHostEnvironment _environment;
        private readonly MySqlConnectionStringBuilder _builder;

        public DbSet<UserTable> Users { get; init; }
        public DbSet<ProfileTable> Profiles { get; init; }

        public ApplicationContext(IOptions<DbConnectionInfo> infoOpt, IWebHostEnvironment environment)
        {
            _environment = environment;
            DbConnectionInfo info = infoOpt.Value;
            _builder = new MySqlConnectionStringBuilder
            {
                Server = info.Server,
                Port = (uint)info.Port,
                UserID = info.UserId,
                Password = info.Password,
                Database = info.Database
            };
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_builder.ToString(), new MySqlServerVersion(new Version(8, 0, 31)));
            if (_environment.IsDevelopment())
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProfileTable>()
                .Navigation(p => p.User)
                .AutoInclude();
        }
    }
}