namespace Infrastructure.GameCycle
{
    public interface IGameStateMachine
    {
        public IGameState Current { get; }
        public void ChangeState<TState>() where TState : IGameState;
    }
}