using UnityEngine;
using UnityEngine.UI;

namespace RoomByRoom.UI.Game.HUD
{
	public class Bar : MonoBehaviour
	{
		protected Slider _slider;

		protected virtual void Awake()
		{
			_slider = GetComponent<Slider>();
		}
		
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