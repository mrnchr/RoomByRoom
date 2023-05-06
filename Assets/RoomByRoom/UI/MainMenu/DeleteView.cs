using UnityEngine;
using UnityEngine.EventSystems;

namespace RoomByRoom.UI.MainMenu
{
  public class DeleteView : MonoBehaviour, IPointerClickHandler
  {
    public delegate void SelectHandler();

    public event SelectHandler OnSelect;

    public void OnPointerClick(PointerEventData eventData) => OnSelect?.Invoke();
  }
}