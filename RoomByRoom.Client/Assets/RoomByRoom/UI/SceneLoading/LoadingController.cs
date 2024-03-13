using System;
using Infrastructure.SceneLoading;
using Zenject;

namespace UI.SceneLoading
{
    public class LoadingController : ILoadingController, IInitializable, IDisposable
    {
        private readonly LoadingView _view;
        private readonly SceneLoadingModel _model;

        public LoadingController(LoadingView view, SceneLoadingModel model)
        {
            _view = view;
            _model = model;

            _model.LoadingProgress.OnChanged += SetValue;
        }

        public void Initialize()
        {
            SetValue();
        }

        public void SetValue()
        {
            _view.SetValue(_model.LoadingProgress);
        }

        public void Dispose()
        {
            _model.LoadingProgress.OnChanged -= SetValue;            
        }
    }
}