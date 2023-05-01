using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom.UI.Game.Inventory
{
	public class SlotInfo : MonoBehaviour
	{
		[field: SerializeField] public bool IsEquipped { get; private set; }
		[field: SerializeField] public WeaponSlotInfo WeaponInfo { get; private set; }
		[field: SerializeField] public ArmorSlotInfo ArmorInfo { get; private set; }

		public void Awake()
		{
			WeaponInfo = GetComponent<WeaponSlotInfo>();
			ArmorInfo = GetComponent<ArmorSlotInfo>();
		}
	}
}