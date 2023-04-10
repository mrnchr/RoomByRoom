using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom
{
	[CreateAssetMenu(menuName = "RoomByRoom/Data/EnemyConfig")]
	public class EnemyConfig : ScriptableObject
	{
		[FormerlySerializedAs("AfterAttackWaitingTime")] [Header("Humanoid")]
		public float DelayAttackTime;

		public float AttackDistance;
	}
}