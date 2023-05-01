using UnityEngine;

namespace RoomByRoom.UI.Game.Inventory
{
	public class ArmorSlotInfo : MonoBehaviour
	{
		[SerializeField] private ArmorType _type;
		public ArmorType Type => _type;
	}
}