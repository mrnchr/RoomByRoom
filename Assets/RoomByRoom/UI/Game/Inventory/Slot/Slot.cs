using System;
using UnityEngine;
using UnityEngine.UI;

namespace RoomByRoom.UI.Game.Inventory
{
	public class Slot : MonoBehaviour
	{
		public bool IsEmpty { get; private set; }
		[field: SerializeField] public SlotInfo Info { get; private set; }
		[SerializeField] private Image _itemImage;

		private void Awake()
		{
			Info = GetComponent<SlotInfo>();
			IsEmpty = true;
		}

		public void SetItem(Sprite item)
		{
			IsEmpty = !item;
			_itemImage.gameObject.SetActive(item);
			_itemImage.sprite = item;
		}
	}
}