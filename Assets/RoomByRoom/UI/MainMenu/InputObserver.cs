using System;
using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
	public class InputObserver : MonoBehaviour
	{
		public delegate void MouseUpHandler(Vector3 mousePosition);
		public event MouseUpHandler OnMouseUp;
		
		public delegate void BackHandler();
		public event BackHandler OnBack;
		
		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Mouse0))
				OnMouseUp?.Invoke(Input.mousePosition);

			if (Input.GetKeyUp(KeyCode.Escape))
				OnBack?.Invoke();
		}
	}
}