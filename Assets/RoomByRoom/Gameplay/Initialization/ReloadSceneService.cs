using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoomByRoom
{
  public class ReloadSceneService
  {
    private AsyncOperation _load;

    public void ReloadScene()
    {
      _load = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
      _load.allowSceneActivation = false;
    }

    public void AllowSceneActivation() => _load.allowSceneActivation = true;
  }
}