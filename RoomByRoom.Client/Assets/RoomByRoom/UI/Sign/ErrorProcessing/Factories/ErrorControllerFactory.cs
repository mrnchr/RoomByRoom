using Zenject;

namespace UI.Menu.ErrorProcessing.Factories
{
    public class ErrorControllerFactory : IErrorControllerFactory
    {
        private readonly DiContainer _container;

        public ErrorControllerFactory(DiContainer container)
        {
            _container = container;
        }

        public IErrorController Create(ErrorView view)
        {
            return _container.Instantiate<ErrorController>(new object[] { view });
        }
    }
}