using SharedData.Checkers;

namespace Authorization
{
    public class CheckingService : ICheckingService
    {
        private readonly IUserInputChecker _checker;
        private readonly AuthorizationModel _model;

        public CheckingService(IUserInputChecker checker, AuthorizationModel model)
        {
            _checker = checker;
            _model = model;
        }

        public bool Check()
        {
            if (!_checker.CheckUserName(_model.Name))
                _model.InputError.Value = InputErrorType.InvalidName;
            else if (!_checker.CheckPasswordLength(_model.Password))
                _model.InputError.Value = InputErrorType.ShortPassword;
            else if (!_checker.CheckPasswordValid(_model.Password))
                _model.InputError.Value = InputErrorType.InvalidPassword;
            else if (!_checker.CheckConfirmedPassword(_model.Password, _model.ConfirmedPassword))
                _model.InputError.Value = InputErrorType.NotConfirmedPassword;
            else
                _model.InputError.Value = InputErrorType.None;

            return _model.InputError == InputErrorType.None;
        }
    }
}