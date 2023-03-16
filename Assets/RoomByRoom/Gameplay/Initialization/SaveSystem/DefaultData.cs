using UnityEngine;

namespace RoomByRoom
{
    [CreateAssetMenu(menuName = "RoomByRoom/Data/DefaultData")]
    public class DefaultData : ScriptableObject
    {
        public GameInfo GameInfo;
        public PlayerEntity Player;
        public RoomEntity Room;
        public InventoryEntity Inventory;
    }
}