namespace RoomByRoom.UI.MainMenu
{
	public class ProfileSelectorBack : WindowBack
	{
		private MainMenuMediator _mainMenuMediator;

		protected override void Awake()
		{
			base.Awake();
			_mainMenuMediator = FindObjectOfType<MainMenuMediator>();
		}

		protected override void HideWindow() => _mainMenuMediator.SwitchProfile(false);
	}
}