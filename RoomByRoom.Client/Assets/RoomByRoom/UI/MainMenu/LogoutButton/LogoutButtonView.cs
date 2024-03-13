using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RoomByRoom.UI.MainMenu.LogoutButtonView
{
  public class LogoutButtonView : MonoBehaviour
  {
    [SerializeField] private Button _button;
    private ILogoutButtonController _controller;

    [Inject]
    public void Construct(ILogoutButtonController controller)
    {
      _controller = controller;
      _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDestroy()
    {
      _button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
      _controller.Logout();
    }
  }
}