using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoomByRoom.UI.Game.HUD
{
  public class WinUISwitcher : MonoBehaviour
  {
    [SerializeField] private GameObject _nextRoomHint;
    [SerializeField] private GameObject _winHint;

    public void SetWin(bool active)
    {
      Debug.Log($"Win: {active}");
      _nextRoomHint.SetActive(active);
      _winHint.SetActive(active);
    }

    public void SetStart(bool active)
    {
      Debug.Log($"Start: {active}");
      _nextRoomHint.SetActive(active);
      _winHint.SetActive(false);
    }
  }
}