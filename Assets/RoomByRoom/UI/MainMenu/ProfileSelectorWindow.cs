using System;
using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
	public class ProfileSelectorWindow : MonoBehaviour
	{
		private MainMenuMediator _mainMenuMediator;

		private void Awake() => _mainMenuMediator = FindObjectOfType<MainMenuMediator>();

		private void Start()
		{
			foreach (string profile in _mainMenuMediator.LoadProfiles())
				_mainMenuMediator.CreateProfileButton().Text.text = profile;
		}

		public void Show() => _mainMenuMediator.SwitchProfile(true);
	}
}