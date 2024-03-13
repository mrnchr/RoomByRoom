namespace Authorization
{
    public interface IAuthorizationProcessor
    {
        public void Register();
        public void Login();
    }
}