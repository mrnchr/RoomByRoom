namespace Server.Database
{
    public class DbConnectionInfo
    {
        public string Server { get; init; } = "";
        public int Port { get; init; }
        public string UserId { get; init; } = "";
        public string Password { get; init; } = "";
        public string Database { get; init; } = "";
    }
}