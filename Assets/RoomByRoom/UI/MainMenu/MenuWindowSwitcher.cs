using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
  public class MenuWindowSwitcher : MonoBehaviour
  {
    [SerializeField] private GameObject _buttonWindow;
    [SerializeField] private GameObject _profileWindow;
    [SerializeField] private GameObject _newProfileWindow;

    public void Awake()
    {
      _buttonWindow.SetActive(true);
      _profileWindow.SetActive(false);
      _newProfileWindow.SetActive(false);
    }

    public void SwitchProfile(bool active)
    {
      _buttonWindow.SetActive(!active);
      _profileWindow.SetActive(active);
    }

    public void SwitchNewProfile(bool active)
    {
      _buttonWindow.SetActive(!active);
      _newProfileWindow.SetActive(active);
    }
  }
}