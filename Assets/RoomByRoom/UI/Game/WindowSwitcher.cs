using System;
using UnityEngine;

namespace RoomByRoom.UI.Game
{
	public class WindowSwitcher : MonoBehaviour
	{
		[SerializeField] private GameObject _hud;
		[SerializeField] private GameObject _pause;
		[SerializeField] private GameObject _inventory;

		private void Awake()
		{
			_hud.SetActive(true);
			_pause.SetActive(false);
			_inventory.SetActive(false);
		}

		public void TurnWindow(WindowType window)
		{
			bool isHud = window == WindowType.HUD;
			bool isPause = window == WindowType.Pause;
			bool isInv = window == WindowType.Inventory;
			
			_hud.SetActive(isHud);
			_pause.SetActive(isPause);
			_inventory.SetActive(isInv);
		}
	}
}