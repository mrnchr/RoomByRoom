namespace UI.Menu.ErrorProcessing.Factories
{
    public interface IErrorControllerFactory
    {
        IErrorController Create(ErrorView view);
    }
}