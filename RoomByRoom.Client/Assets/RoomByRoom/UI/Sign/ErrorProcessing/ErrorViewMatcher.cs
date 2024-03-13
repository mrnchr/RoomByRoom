using Authorization;
using SharedData.Authentication;

namespace UI.Menu.ErrorProcessing
{
    public class ErrorViewMatcher : IErrorViewMatcher
    {
        public bool Match(ErrorViewType view, AuthenticationErrorType error)
        {
            return view == ErrorViewType.Login && error == AuthenticationErrorType.InvalidLoginOrPassword ||
                view == ErrorViewType.RegisterName && error == AuthenticationErrorType.ExistingLogin;
        }

        public bool Match(ErrorViewType view, InputErrorType input)
        {
            return view == ErrorViewType.RegisterName && input == InputErrorType.InvalidName ||
                view == ErrorViewType.RegisterPassword &&
                input is InputErrorType.InvalidPassword or InputErrorType.ShortPassword ||
                view == ErrorViewType.RegisterConfirmedPassword && input == InputErrorType.NotConfirmedPassword;
        }
    }
}