using UnityEngine;

namespace RoomByRoom
{
	[CreateAssetMenu(menuName = "RoomByRoom/Data/EnemyData")]
	public class EnemyData : ScriptableObject
	{
		public ArmorData Armor;

		[Header("Humanoid")]
		public float DelayAttackTime;

		public float AttackDistance;
	}
}