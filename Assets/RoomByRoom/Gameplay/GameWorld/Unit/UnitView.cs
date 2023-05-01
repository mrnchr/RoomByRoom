using RoomByRoom.UI.Game.HUD;
using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom
{
	[RequireComponent(typeof(Animator), typeof(AttackCatcher))]
	public class UnitView : View
	{
		[FormerlySerializedAs("MovingCmp")] public Movable MovableCmp;
		[HideInInspector] public Animator Anim;
		[HideInInspector] public AttackCatcher AttackCtr;
		[HideInInspector] public Rigidbody Rb;
		public Bar HealthBar;
		public Bar ArmorBar;

		protected virtual void Awake()
		{
			Rb = GetComponent<Rigidbody>();
			Anim = GetComponent<Animator>();
			AttackCtr = GetComponent<AttackCatcher>();

			OnAwake();
		}

		protected virtual void OnAwake()
		{
		}

		public virtual void StartAttackAnimation()
		{
		}
	}
}