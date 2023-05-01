using System;
using UnityEngine;

namespace RoomByRoom
{
	public class SpriteService
	{
		public readonly SpriteData Sprites;

		public SpriteService(SpriteData spriteData) => Sprites = spriteData;

		public Sprite GetItem(ItemType type, int eqType, int index = -1) =>
			index == -1
				? Sprites.PlayerHand
				: GetItems(type, eqType)[index];

		public Sprite[] GetItems(ItemType item, int eqType) =>
			item switch
			{
				ItemType.Armor => GetArmors((ArmorType)eqType),
				ItemType.Artifact => Sprites.Artifacts,
				ItemType.Weapon => GetWeapons((WeaponType)eqType),
				_ => throw new ArgumentOutOfRangeException()
			};

		private Sprite[] GetArmors(ArmorType type) =>
			type switch
			{
				ArmorType.Boots => Sprites.Boots,
				ArmorType.BreastPlate => Sprites.Breastplates,
				ArmorType.Gloves => Sprites.Gloves,
				ArmorType.Helmet => Sprites.Helmets,
				ArmorType.Leggings => Sprites.Leggings,
				ArmorType.Shield => Sprites.Shields,
				_ => throw new ArgumentOutOfRangeException()
			};

		private Sprite[] GetWeapons(WeaponType type) =>
			type switch
			{
				WeaponType.Bow => Sprites.Bows,
				WeaponType.OneHand => Sprites.OneHands,
				WeaponType.TwoHands => Sprites.TwoHands,
				_ => throw new ArgumentOutOfRangeException()
			};
	}
}