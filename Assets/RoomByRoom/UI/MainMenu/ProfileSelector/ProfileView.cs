using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RoomByRoom.UI.MainMenu
{
  public class ProfileView : MonoBehaviour, IPointerClickHandler
  {
    [field: SerializeField] public TMP_Text Text { get; private set; }
    
    public delegate void SelectHandler(string name);
    public event SelectHandler OnSelect;

    public void OnPointerClick(PointerEventData eventData) => OnSelect?.Invoke(Text.text);
  }
}