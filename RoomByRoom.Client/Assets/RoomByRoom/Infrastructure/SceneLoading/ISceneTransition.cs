namespace Infrastructure.SceneLoading
{
    public interface ISceneTransition
    {
        public string LoadingScene { get; }
        public void Transit(string scene);
    }
}