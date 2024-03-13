using SharedData.Checkers;

namespace Server.Authentication
{
    public class CheckingService : ICheckingService
    {
        private readonly IUserInputChecker _checker;

        public CheckingService(IUserInputChecker checker)
        {
            _checker = checker;
        }

        public bool Check(string name, string password)
        {
            return _checker.CheckUserName(name) && _checker.CheckPasswordLength(password) &&
                _checker.CheckPasswordValid(password);
        }
    }
}