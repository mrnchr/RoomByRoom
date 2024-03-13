using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RoomByRoom.UI.MainMenu.StartButton
{
  public class StartGameButtonView : MonoBehaviour
  {
    [SerializeField] private Button _button;
    private IStartGameButtonController _controller;

    [Inject]
    public void Construct(IStartGameButtonController controller)
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
      _controller.StartGame();
    }
  }
}