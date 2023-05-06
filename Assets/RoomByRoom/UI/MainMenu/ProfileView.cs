using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RoomByRoom.UI.MainMenu
{
  public class ProfileView : MonoBehaviour, IPointerClickHandler
  {
    public delegate void SelectHandler(string name);

    public event SelectHandler OnSelect;

    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _button;

    public TMP_Text Text => _text;
    public Button Button => _button;

    public void OnPointerClick(PointerEventData eventData) => OnSelect?.Invoke(_text.text);
  }
}