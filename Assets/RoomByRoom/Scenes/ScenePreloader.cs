using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoomByRoom.Scene
{
  public class ScenePreloader
  {
    private AsyncOperation _load;

    public void PreloadScene(int buildIndex)
    {
      _load = SceneManager.LoadSceneAsync(buildIndex);
      _load.allowSceneActivation = false;
    }

    public void AllowSceneActivation() => _load.allowSceneActivation = true;
  }
}