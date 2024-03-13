using Configuration;

namespace Infrastructure.SceneLoading
{
    public class SceneLoadingService : ISceneLoadingService
    {
        private readonly SceneConfig _config;
        private readonly ISceneTransition _transition;

        public SceneLoadingService(SceneConfig config, ISceneTransition transition)
        {
            _config = config;
            _transition = transition;
        }

        public void LoadSignScene()
        {
            _transition.Transit(_config.SignScene);
        }

        public void LoadMainScene()
        {
            _transition.Transit(_config.MainScene);
        }

        public void LoadGameScene()
        {
            _transition.Transit(_config.GameScene);
        }
    }
}