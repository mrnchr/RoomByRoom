using Configuration;
using Infrastructure.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.SceneLoading
{
    public class SceneTransition : ISceneTransition
    {
        private readonly SceneConfig _config;
        private readonly IPersonalLogger _logger;
        private AsyncOperation _operation;
        
        public string LoadingScene { get; private set; }

        public SceneTransition(SceneConfig config)
        {
            _config = config;
        }
        
        public void Transit(string scene)
        {
            SceneManager.LoadScene(_config.LoadingScene);
            LoadingScene = scene;
        }
    }
}