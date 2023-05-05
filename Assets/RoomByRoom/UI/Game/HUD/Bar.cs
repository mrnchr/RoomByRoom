using UnityEngine;
using UnityEngine.UI;

namespace RoomByRoom.UI.Game.HUD
{
	public class Bar : MonoBehaviour
	{
		[SerializeField] protected Slider _slider;
		
		public virtual void SetMaxValue(float maxValue)
		{
			_slider.maxValue = maxValue;
		}

		public virtual void SetValue(float value)
		{
			_slider.value = value;
		}
	}
}