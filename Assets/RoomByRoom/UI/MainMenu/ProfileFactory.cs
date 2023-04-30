using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
	public class ProfileButtonFactory : IFactory<ProfileView>
	{
		private readonly ProfileView _prefab;
		private readonly Transform _parent;
		private readonly Mediator _mediator;

		public ProfileButtonFactory(ProfileView prefab, Transform parent, Mediator mediator)
		{
			_prefab = prefab;
			_parent = parent;
			_mediator = mediator;
		}

		public ProfileView Create()
		{
			ProfileView view = Object.Instantiate(_prefab, _parent);
			view.OnSelect += _mediator.StartGame;

			return view;
		}
	}
}