namespace RoomByRoom.UI.MainMenu
{
	public class ProfileSelectorBack : WindowBack
	{
		private Mediator _mediator;

		protected override void Awake()
		{
			base.Awake();
			_mediator = FindObjectOfType<Mediator>();
		}

		protected override void HideWindow() => _mediator.SwitchProfile(false);
	}
}