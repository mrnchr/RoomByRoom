using UnityEngine;

namespace RoomByRoom
{
	public class WeaponView : ItemView
	{
		public Collider[] AttackTriggers;

		private void Awake()
		{
			SetActiveAttackTriggers(false);
		}

		public void SetActiveAttackTriggers(bool isActive)
		{
			foreach (Collider trigger in AttackTriggers) 
				trigger.enabled = isActive;
		}
	}
}