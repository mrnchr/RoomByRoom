using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.SceneLoading
{
    public class SceneLoader : ISceneLoader, IInitializable
    {
        private readonly ISceneTransition _transition;
        private readonly SceneLoadingModel _model;
        private readonly ICoroutineRunner _runner;
        private AsyncOperation _operation;

        public SceneLoader(ISceneTransition transition, SceneLoadingModel model, ICoroutineRunner runner)
        {
            _transition = transition;
            _model = model;
            _runner = runner;
        }
        
        public void Initialize()
        {
            LoadScene(_transition.LoadingScene);
        }

        public void LoadScene(string sceneName)
        {
            _operation = SceneManager.LoadSceneAsync(sceneName);
            _runner.Run(UpdateLoadingProgress());
        }

        private IEnumerator UpdateLoadingProgress()
        {
            while (!_operation.isDone)
            {
                _model.LoadingProgress.Value = _operation.progress;
                yield return null;
            }
        }
    }
}