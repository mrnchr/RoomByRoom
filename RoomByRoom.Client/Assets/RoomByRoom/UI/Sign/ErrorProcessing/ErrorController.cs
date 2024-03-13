using System;
using Authorization;
using Configuration;
using Zenject;

namespace UI.Menu.ErrorProcessing
{
    public class ErrorController : IErrorController, IDisposable
    {
        private readonly ErrorView _view;
        private readonly AuthorizationErrorConfig _config;
        private readonly AuthorizationModel _model;
        private readonly IErrorViewMatcher _matcher;

        public ErrorController(ErrorView view,
            AuthorizationErrorConfig config,
            AuthorizationModel model,
            IErrorViewMatcher matcher)
        {
            _view = view;
            _config = config;
            _model = model;
            _matcher = matcher;

            _model.AuthError.OnChanged += ProcessAuthError;
            _model.InputError.OnChanged += ProcessInputError;
        }

        [Inject]
        public void Initialize()
        {
            _view.SetText("");
        }

        public void ProcessAuthError()
        {
            _view.SetText(_matcher.Match(_view.Id, _model.AuthError) ? _config.Get(_model.AuthError) : "");
        }

        public void ProcessInputError()
        {
            _view.SetText(_matcher.Match(_view.Id, _model.InputError) ? _config.Get(_model.InputError) : "");
        }

        public void Dispose()
        {
            _model.AuthError.OnChanged -= ProcessAuthError;
            _model.InputError.OnChanged -= ProcessInputError;
        }
    }
}