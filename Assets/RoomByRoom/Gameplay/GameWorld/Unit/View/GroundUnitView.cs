using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom
{
	public class GroundUnitView : UnitView
	{
		[FormerlySerializedAs("JumpingCmp")] public Jumpable JumpableCmp;
		public LayerMask GroundMask;
	}
}