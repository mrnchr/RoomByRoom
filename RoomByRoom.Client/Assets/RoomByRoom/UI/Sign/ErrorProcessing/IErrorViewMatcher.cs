using Authorization;
using SharedData.Authentication;

namespace UI.Menu.ErrorProcessing
{
    public interface IErrorViewMatcher
    {
        bool Match(ErrorViewType view, AuthenticationErrorType error);
        bool Match(ErrorViewType view, InputErrorType input);
    }
}