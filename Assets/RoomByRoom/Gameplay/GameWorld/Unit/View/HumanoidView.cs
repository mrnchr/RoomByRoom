using System;
using UnityEngine;

namespace RoomByRoom
{
	[Serializable]
	public struct ItemPlace
	{
		public Transform Parent;
		public Transform Point;
	}

	[Serializable]
	public struct WeaponPlace
	{
		public WeaponType Type;
		public ItemPlace Place;
	}

	[Serializable]
	public struct ArmorPlace
	{
		public ArmorType Type;
		public ItemPlace Place;
	}

	public class HumanoidView : GroundUnitView
	{
		private static readonly int _weapon = Animator.StringToHash("Weapon");
		private static readonly int _startAttack = Animator.StringToHash("StartAttack");
		[SerializeField] protected WeaponPlace[] WeaponPlaces;
		[SerializeField] protected ArmorPlace[] ArmorPlaces;

		public ItemPlace GetWeaponPlace(WeaponType type)
		{
			return Array.Find(WeaponPlaces, x => x.Type == type).Place;
		}

		public ItemPlace GetArmorPlace(ArmorType type)
		{
			return Array.Find(ArmorPlaces, x => x.Type == type).Place;
		}

		public override void PlayAttackAnimation(WeaponType weaponType)
		{
			Anim.SetInteger(_weapon, (int)weaponType);
			Anim.SetTrigger(_startAttack);
			// TODO: change when there is a bow
		}
	}
}