using UnityEngine.SceneManagement;

namespace RoomByRoom.UI.MainMenu
{
  public class StartGameService
  {
    private readonly OuterData _outerData;

    public StartGameService(OuterData outerData)
    {
      _outerData = outerData;
    }

    public void StartGame(string profileName)
    {
      _outerData.ProfileName = profileName;
      SceneManager.LoadScene(2);
    }
  }
}