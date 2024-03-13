using System;
using System.Collections.Generic;
using UI.Menu.ErrorProcessing;
using UI.Menu.ErrorProcessing.Factories;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace UI.Menu
{
    public class SignInitializer : IInitializable, IDisposable
    {
        private readonly List<IErrorController> _controllers = new List<IErrorController>();
        private readonly IErrorControllerFactory _factory;
        private readonly List<ErrorView> _views;

        public SignInitializer(IErrorControllerFactory factory)
        {
            _factory = factory;
            _views = new List<ErrorView>(Object.FindObjectsByType<ErrorView>(FindObjectsInactive.Include,
                FindObjectsSortMode.None));
        }
        
        public void Initialize()
        {
            foreach (ErrorView view in _views)
            {
                _controllers.Add(_factory.Create(view));
            }
        }

        public void Dispose()
        {
            foreach (IErrorController controller in _controllers)
            {
                (controller as IDisposable)?.Dispose();
            }
        }
    }
}