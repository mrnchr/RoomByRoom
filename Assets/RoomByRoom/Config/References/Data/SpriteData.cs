using UnityEngine;

namespace RoomByRoom
{
	[CreateAssetMenu(menuName = "RoomByRoom/Data/SpriteData")]
	public class SpriteData : ScriptableObject
	{
		public Sprite PlayerHand;
		public Sprite[] OneHands;
		public Sprite[] TwoHands;
		public Sprite[] Bows;
		public Sprite[] Boots;
		public Sprite[] Leggings;
		public Sprite[] Gloves;
		public Sprite[] Breastplates;
		public Sprite[] Helmets;
		public Sprite[] Shields;
		public Sprite[] Artifacts;
	}
}