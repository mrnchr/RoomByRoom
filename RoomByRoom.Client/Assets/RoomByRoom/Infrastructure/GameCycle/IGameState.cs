namespace Infrastructure.GameCycle
{
    public interface IGameState
    {
        public void Enter();
        public void Exit();
    }
}