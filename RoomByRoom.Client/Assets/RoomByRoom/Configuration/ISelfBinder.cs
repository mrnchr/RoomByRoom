using Zenject;

namespace Configuration
{
    public interface ISelfBinder<TSelf> : ISelfBinder where TSelf: class
    {
        public void BindSelf(DiContainer container)
        {
            container.Bind<TSelf>().FromInstance(this as TSelf);
        }
    }

    public interface ISelfBinder
    {
        public void Bind(DiContainer container);
    }
}