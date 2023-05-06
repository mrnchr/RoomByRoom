using System.Collections;
using TMPro;
using UnityEngine;

namespace RoomByRoom.Utility
{
  public class FPSMonitor : MonoBehaviour
  {
    [SerializeField] private TMP_Text _fpsText;
    private float _fps;

    protected void Start()
    {
      _fpsText.rectTransform.sizeDelta = new Vector2(150, 50);
      StartCoroutine(WaitForUpdateFPS());
    }

    protected void Update()
    {
      _fps = 1 / Time.unscaledDeltaTime;
    }

    private IEnumerator WaitForUpdateFPS()
    {
      while (true)
      {
        yield return new WaitForSeconds(0.5f);
        _fpsText.text = $"FPS: {Mathf.RoundToInt(_fps):0.0}";
      }
    }
  }
}