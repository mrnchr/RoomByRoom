using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RoomByRoom.UI.MainMenu
{
  public class WindowBack : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
  {
    private InputObserver _input;
    private bool _isWithinWindow;

    protected virtual void Awake()
    {
      _input = FindObjectOfType<InputObserver>();
    }

    private void OnEnable()
    {
      StartCoroutine(WaitEnable());
    }

    private IEnumerator WaitEnable()
    {
      yield return null;
      _input.OnMouseUp += DoIfIsOutOfBounds;
      _input.OnBack += HideWindow;
    }

    private void OnDisable()
    {
      _input.OnMouseUp -= DoIfIsOutOfBounds;
      _input.OnBack -= HideWindow;
    }

    private void DoIfIsOutOfBounds(Vector3 mousePosition)
    {
      if (!_isWithinWindow)
        HideWindow();
    }

    protected virtual void HideWindow()
    {
    }

    public void OnPointerEnter(PointerEventData eventData) => _isWithinWindow = true;
    public void OnPointerExit(PointerEventData eventData) => _isWithinWindow = false;
  }
}