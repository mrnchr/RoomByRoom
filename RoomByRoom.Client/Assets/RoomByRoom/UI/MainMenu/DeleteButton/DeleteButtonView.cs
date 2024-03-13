using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RoomByRoom.UI.MainMenu.DeleteButton
{
  public class DeleteButtonView : MonoBehaviour
  {
    [SerializeField] private Button _button;
    private IDeleteButtonController _controller;

    [Inject]
    public void Construct(IDeleteButtonController controller)
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
      _controller.Delete();
    }
  }
}