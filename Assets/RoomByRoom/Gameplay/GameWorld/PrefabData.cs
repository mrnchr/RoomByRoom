using System.Collections.Generic;
using UnityEngine;

namespace RoomByRoom
{
    [CreateAssetMenu(menuName = "RoomByRoom/Data/PrefabData")]
    public class PrefabData : ScriptableObject
    {
        public PlayerView[] PlayerViews;
        public RoomView StartRoom;
        public RoomView[] SandEnemyRooms;
        public RoomView[] WaterEnemyRooms;
        public RoomView[] DarkEnemyRooms;
        public RoomView[] BossRooms;


    }
}