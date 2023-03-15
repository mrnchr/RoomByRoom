using UnityEngine;

namespace RoomByRoom
{
    [CreateAssetMenu(menuName = "RoomByRoom/ForDevelopers/DataForStore")]
    public class DataForStore : ScriptableObject
    {
        public int[] BoundEntities;
        public BoundComponent<UnitInfo> player;
    }
}
