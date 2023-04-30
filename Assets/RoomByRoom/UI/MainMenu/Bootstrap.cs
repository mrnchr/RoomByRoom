﻿using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
	[DefaultExecutionOrder(-1)]
	public class Bootstrap : MonoBehaviour
	{
		[Header("Profile Creator Window Construction")]
		[SerializeField] private ProfileCreatorWindow _profileCreatorWindow;
		[SerializeField] private ProfileFieldView _profileFieldView;
		[SerializeField] private GameObject _errorMessageObject;
		
		[Header("Window Switcher Construction")]
		[SerializeField] private WindowSwitcher _windowSwitcher;
		[SerializeField] private GameObject _buttonWindow;
		[SerializeField] private GameObject _profileWindow;
		[SerializeField] private GameObject _newProfileWindow;

		[Header("Mediator Construction")]
		[SerializeField] private Mediator _mediator;
		
		[Header("Profile Button Factory Construction")]
		[SerializeField] private ProfileView _profileButton;
		[SerializeField] private Transform _profileGroup;
		
		
		[Header("Others")]
		[SerializeField] private Configuration _config;
		[SerializeField] private OuterData _outerData;
		
		private StartGameService _startGameSvc;
		private ProfileService _profileSvc;
		private ProfileButtonFactory _profileButtonFactory;

		private void Awake()
		{
			_outerData.transform.SetParent(null);
			DontDestroyOnLoad(_outerData);
			_profileSvc = new ProfileService(_config.SaveInFile);
			_profileButtonFactory = new ProfileButtonFactory(_profileButton, _profileGroup, _mediator);
			_startGameSvc = new StartGameService(_outerData);
			
			_profileCreatorWindow.Construct(_config, _profileFieldView, _errorMessageObject);
			_windowSwitcher.Construct(_buttonWindow, _profileWindow, _newProfileWindow);
			_mediator.Construct(_profileButtonFactory, _profileSvc, _startGameSvc);
		}
	}
}