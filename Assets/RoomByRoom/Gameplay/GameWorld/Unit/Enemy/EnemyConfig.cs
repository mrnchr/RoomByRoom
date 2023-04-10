using UnityEngine;

namespace RoomByRoom
{
	[CreateAssetMenu(menuName = "RoomByRoom/Data/EnemyConfig")]
	public class EnemyConfig : ScriptableObject
	{
		[Header("Humanoid")]
		public float AfterAttackWaitingTime;

		public float AttackDistance;
	}
}