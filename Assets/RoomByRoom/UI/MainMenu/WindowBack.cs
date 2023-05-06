using System.Collections;
using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
  public class WindowBack : MonoBehaviour
  {
    private RectTransform _rect;
    private InputObserver _input;

    protected virtual void Awake()
    {
      _rect = GetComponent<RectTransform>();
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
      var corners = new Vector3[4];
      _rect.GetWorldCorners(corners);

      if (!IsInRect(mousePosition, corners[0], corners[2]))
        HideWindow();
    }

    protected virtual void HideWindow()
    {
    }

    private static bool IsInRect(Vector3 point, Vector3 nearCorner, Vector3 farCorner) =>
      IsInBounds(point.x, nearCorner.x, farCorner.x)
      && IsInBounds(point.y, nearCorner.y, farCorner.y);

    private static bool IsInBounds(float value, float left, float right) => value >= left && value <= right;
  }
}