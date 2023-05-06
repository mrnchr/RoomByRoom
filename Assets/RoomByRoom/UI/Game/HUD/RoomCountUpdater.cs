using System;
using TMPro;
using UnityEngine;

namespace RoomByRoom.UI.Game.HUD
{
  public class RoomCountUpdater : MonoBehaviour
  {
    [SerializeField] private TMP_Text _text;

    private void Awake()
    {
      _text.text = "";
    }

    public void SetText(string text)
    {
      _text.text = text;
    }
  }
}