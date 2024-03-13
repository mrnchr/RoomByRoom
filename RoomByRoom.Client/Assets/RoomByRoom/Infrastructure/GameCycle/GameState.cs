namespace Infrastructure.GameCycle
{
    public abstract class GameState : IGameState
    {
        public abstract void Enter();
        public abstract void Exit();
    }
}