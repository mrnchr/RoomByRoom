using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RoomByRoom.UI.MainMenu.ContinueButton
{
  public class ContinueGameButtonView : MonoBehaviour
  {
    [SerializeField] private Button _button; 
    private IContinueGameButtonController _controller;

    [Inject]
    public void Construct(IContinueGameButtonController controller)
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
      _controller.Continue();      
    }
  }
}