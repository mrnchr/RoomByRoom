using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Infrastructure.GameCycle
{
    public class GameStateMachine : IGameStateMachine, IInitializable
    {
        private readonly IGameStateFactory _factory;
        private readonly List<IGameState> _states = new List<IGameState>();
        private IGameState _current;

        public IGameState Current => _current;

        public GameStateMachine(IGameStateFactory factory)
        {
            _factory = factory;
        }

        public void Initialize()
        {
            _states.AddRange(new IGameState[]
            {
                _factory.Create<EmptyGameState>(),
                _factory.Create<SceneLoadingState>()
            });
        }

        public void ChangeState<TState>() where TState : IGameState
        {
            _current?.Exit();
            
            _current = _states.OfType<TState>().First();
            _current?.Enter();
        }
    }
}