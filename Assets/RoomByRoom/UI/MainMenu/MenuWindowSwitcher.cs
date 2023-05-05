using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
	public class MenuWindowSwitcher : MonoBehaviour
	{
		private GameObject _buttonWindow;
		private GameObject _profileWindow;
		private GameObject _newProfileWindow;

		public void Construct(GameObject buttonWindow, GameObject profileWindow, GameObject newProfileWindow)
		{
			_buttonWindow = buttonWindow;
			_profileWindow = profileWindow;
			_newProfileWindow = newProfileWindow;

			_buttonWindow.SetActive(true);
			_profileWindow.SetActive(false);
			_newProfileWindow.SetActive(false);
		}
		
		public void SwitchProfile(bool isProfileActive)
		{
			_buttonWindow.SetActive(!isProfileActive);
			_profileWindow.SetActive(isProfileActive);
		}

		public void SwitchNewProfile(bool isProfileActive)
		{
			_buttonWindow.SetActive(!isProfileActive);
			_newProfileWindow.SetActive(isProfileActive);
		}
	}
}