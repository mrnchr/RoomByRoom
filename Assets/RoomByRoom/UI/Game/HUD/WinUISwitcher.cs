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
      _nextRoomHint.SetActive(active);
      _winHint.SetActive(active);
    }

    public void SetStart(bool active)
    {
      _nextRoomHint.SetActive(active);
      _winHint.SetActive(false);
    }
  }
}