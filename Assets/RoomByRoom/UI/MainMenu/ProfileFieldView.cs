using System;
using TMPro;
using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
	public class ProfileFieldView : MonoBehaviour
	{
		public delegate void ChangeHandler(string text);

		public event ChangeHandler OnValueChanged;
		public TMP_InputField InputField { get; private set; }

		private void Awake()
		{
			InputField = GetComponent<TMP_InputField>();
		}

		public void OnFieldChanged() => OnValueChanged?.Invoke(InputField.text);
	}
}