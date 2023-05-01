using System.Collections.Generic;
using RoomByRoom.UI.Game.Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoomByRoom.UI.Game
{
	public class GameMediator : MonoBehaviour
	{
		private WindowSwitcher _windowSwitcher;
		private TurnWindowService _turnWindowSvc;
		private InventoryKeeper _inventory;

		public void Construct(TurnWindowService turnWindowSvc)
		{
			_turnWindowSvc = turnWindowSvc;
		}

		private void Awake()
		{
			_windowSwitcher = FindObjectOfType<WindowSwitcher>();
			_inventory = FindObjectOfType<InventoryKeeper>();
		}

		public void TurnWindow(WindowType window) => _windowSwitcher.TurnWindow(window);
		public void ContinueGame() => _turnWindowSvc.SendSwitchWindowMessage(SwitchWindowMessageType.Pause);
		public void ExitToMenu() => SceneManager.LoadScene(0);
		public void UpdateInventory(List<int> inventory) => _inventory.UpdateInventory(inventory);
	}
}