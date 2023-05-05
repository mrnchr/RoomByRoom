using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoomByRoom
{
	[Serializable]
	public struct WeaponPlace
	{
		public WeaponType Type;
		public Transform ItemPlace;
	}

	[Serializable]
	public struct ArmorPlace
	{
		public ArmorType Type;
		public Transform ItemPlace;
	}

	public class HumanoidView : GroundUnitView
	{
		private static readonly int _weapon = Animator.StringToHash("Weapon");
		private static readonly int _startAttack = Animator.StringToHash("StartAttack");
		[SerializeField] protected WeaponPlace[] WeaponPlaces;
		[SerializeField] protected ArmorPlace[] ArmorPlaces;
		private readonly List<Animator> _armorAnimators = new List<Animator>();

		public void AddArmorAnimator(Animator armorAnim)
		{
			_armorAnimators.Add(armorAnim);
			SyncAnimParameters(armorAnim);
		}

		public void RemoveArmorAnimator(Animator armorAnim)
		{
			_armorAnimators.Remove(armorAnim);
		}

		private void SyncAnimParameters(Animator anim)
		{
			anim.SetBool(_isRunning, Anim.GetBool(_isRunning));	
			anim.SetBool(_isJumping, Anim.GetBool(_isJumping));	
			anim.SetInteger(_weapon, Anim.GetInteger(_weapon));
		}

		public Transform GetWeaponPlace(WeaponType type) => Array.Find(WeaponPlaces, x => x.Type == type).ItemPlace;
		public Transform GetArmorPlace(ArmorType type) => Array.Find(ArmorPlaces, x => x.Type == type).ItemPlace;
		public virtual void SetWeaponToAnimate(WeaponType type)
		{
			Anim.SetInteger(_weapon, (int)type);
			_armorAnimators.ForEach(x => x.SetInteger(_weapon, (int)type));
		}

		public override void AnimateJump(bool isJump)
		{
			base.AnimateJump(isJump);
			_armorAnimators.ForEach(x => x.SetBool(_isJumping, isJump));
		}

		public override void AnimateRun(bool isRun)
		{
			base.AnimateRun(isRun);
			_armorAnimators.ForEach(x => x.SetBool(_isRunning, isRun));
		}

		// TODO: change when there is a bow
		public override void StartAttackAnimation()
		{
			Anim.SetTrigger(_startAttack);
			_armorAnimators.ForEach(x => x.SetTrigger(_startAttack));
		}

		private void Reset()
		{
			WeaponPlaces = new WeaponPlace[4];
			for (int i = 0; i < WeaponPlaces.Length; i++)
				WeaponPlaces[i].Type = (WeaponType)i;

			ArmorPlaces = new ArmorPlace[6];
			for (int i = 0; i < ArmorPlaces.Length; i++)
				ArmorPlaces[i].Type = (ArmorType)i;
		}
	}
}