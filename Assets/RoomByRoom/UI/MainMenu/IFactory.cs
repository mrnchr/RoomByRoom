using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
	public interface IFactory<out T>
	{
		public T Create();
	}

	public interface IFactory
	{
		public GameObject Create();
	}
}