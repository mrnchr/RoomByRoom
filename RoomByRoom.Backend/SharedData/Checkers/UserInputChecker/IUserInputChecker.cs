namespace SharedData.Checkers
{
    public interface IUserInputChecker
    {
        public bool CheckUserName(string name);
        public bool CheckPasswordLength(string password);
        public bool CheckPasswordValid(string password);
        public bool CheckConfirmedPassword(string password, string confirmedPassword);
    }
}