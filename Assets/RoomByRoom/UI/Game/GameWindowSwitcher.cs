using System;
using UnityEngine;

namespace RoomByRoom.UI.Game
{
  public class GameWindowSwitcher : MonoBehaviour
  {
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _win;
    [SerializeField] private GameObject _lose;

    private void Awake()
    {
      Deactivate();
    }

    private void Deactivate()
    {
      _hud.SetActive(false);
      _pause.SetActive(false);
      _inventory.SetActive(false);
      _win.SetActive(false);
      _lose.SetActive(false);
    }

    public void TurnWindow(WindowType window)
    {
      Deactivate();
      switch (window)
      {
        case WindowType.None: break;
        case WindowType.HUD:
          _hud.SetActive(true);
          break;
        case WindowType.Inventory:
          _inventory.SetActive(true);
          break;
        case WindowType.Pause:
          _pause.SetActive(true);
          break;
        case WindowType.Win:
          _win.SetActive(true);
          break;
        case WindowType.Lose:
          _lose.SetActive(true);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(window), window, null);
      }
    }
  }
}