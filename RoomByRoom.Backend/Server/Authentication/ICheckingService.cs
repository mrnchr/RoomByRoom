namespace Server.Authentication
{
    public interface ICheckingService
    {
        public bool Check(string name, string password);
    }
}