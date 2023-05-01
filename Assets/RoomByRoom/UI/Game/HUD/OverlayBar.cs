using TMPro;
using UnityEngine;

namespace RoomByRoom.UI.Game.HUD
{
	public class OverlayBar : Bar
	{
		[SerializeField] protected TMP_Text _valueText;

		public override void SetMaxValue(float maxValue)
		{
			base.SetMaxValue(maxValue);
			SetText();
		}

		public override void SetValue(float value)
		{
			base.SetValue(value);
			SetText();
		}

		private void SetText() => SetText(Mathf.RoundToInt(_slider.value), Mathf.RoundToInt(_slider.maxValue));
		private void SetText(int value, int maxValue) => _valueText.text = $"{value}/{maxValue}";
	}
}