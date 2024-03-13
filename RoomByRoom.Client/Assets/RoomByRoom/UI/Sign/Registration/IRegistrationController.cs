namespace UI.Menu.Registration
{
    public interface IRegistrationController
    {
        public void Register(string login, string password, string confirmedPassword);
    }
}