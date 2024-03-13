namespace Infrastructure.GameCycle
{
    public interface IGameStateFactory
    {
        TState Create<TState>() where TState : IGameState;
    }
}