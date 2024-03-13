using Zenject;

namespace Infrastructure.GameCycle
{
    public class GameStateFactory : IGameStateFactory
    {
        private readonly DiContainer _container;

        public GameStateFactory(DiContainer container)
        {
            _container = container;
        }

        public TState Create<TState>() where TState : IGameState
        {
            return _container.Instantiate<TState>();
        }
    }
}