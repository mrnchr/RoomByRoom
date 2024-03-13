namespace SharedData.Checkers
{
    public class UserInputChecker : IUserInputChecker
    {
        private const int LENGTH = 8;
        private const string INVALID_PASSWORD_SYMBOLS = " ";
        private const string INVALID_USER_NAME_SYMBOLS = "(){}[]|`! \"$%^&*\"<>:;#~+=,@";

        public bool CheckUserName(string name)
        {
            return !string.IsNullOrEmpty(name) && name.IndexOfAny(INVALID_USER_NAME_SYMBOLS.ToCharArray()) == -1;
        }

        public bool CheckPasswordLength(string password)
        {
            return password.Length >= LENGTH;
        }

        public bool CheckPasswordValid(string password)
        {
            return password.IndexOfAny(INVALID_PASSWORD_SYMBOLS.ToCharArray()) == -1;
        }

        public bool CheckConfirmedPassword(string password, string confirmedPassword)
        {
            return password == confirmedPassword;
        }
    }
}