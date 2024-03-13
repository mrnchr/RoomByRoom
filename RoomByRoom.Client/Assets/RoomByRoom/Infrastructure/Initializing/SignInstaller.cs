using Authorization;
using SharedData.Checkers;
using UI.Menu.ErrorProcessing;
using UI.Menu.ErrorProcessing.Factories;
using UI.Menu.Registration;
using Zenject;

namespace UI.Menu
{
    public class SignInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindModel();
            BindChecking();
            BindAuthorizationProcessor();
            BindControllers();

            BindErrorProcessing();
        }

        private void BindErrorProcessing()
        {
            Container.Bind<IErrorViewMatcher>().To<ErrorViewMatcher>().AsSingle();
            Container.Bind<IErrorControllerFactory>().To<ErrorControllerFactory>().AsSingle();
            Container.BindInterfacesTo<SignInitializer>().AsSingle();
        }

        private void BindControllers()
        {
            Container.Bind<ILoginController>().To<LoginController>().AsSingle();
            Container.Bind<IRegistrationController>().To<RegistrationController>().AsSingle();
        }

        private void BindAuthorizationProcessor()
        {
            Container.Bind<IAuthorizationProcessor>().To<AuthorizationProcessor>().AsSingle();
        }

        private void BindModel()
        {
            Container.Bind<AuthorizationModel>().AsSingle();
        }

        private void BindChecking()
        {
            Container.Bind<IUserInputChecker>().To<UserInputChecker>().AsSingle();
            Container.Bind<ICheckingService>().To<CheckingService>().AsSingle();
        }
    }
}