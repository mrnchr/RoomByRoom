using System;
using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
	public class ProfileSelectorWindow : MonoBehaviour
	{
		private Mediator _mediator;

		private void Awake() => _mediator = FindObjectOfType<Mediator>();

		private void Start()
		{
			foreach (string profile in _mediator.LoadProfiles())
				_mediator.CreateProfileButton().Text.text = profile;
		}

		public void Show() => _mediator.SwitchProfile(true);
	}
}