using System.Collections.Generic;
using UnityEngine;

namespace RoomByRoom
{
    [CreateAssetMenu(menuName = "RoomByRoom/Data/PrefabData")]
    public class PrefabData : ScriptableObject
    {
        public PlayerView BasePlayerUnit;
        public UnitView[] SandEnemyUnits;
        public UnitView[] WaterEnemyUnits;
        public UnitView[] DarkEnemyUnits;
        public UnitView[] BossUnits;
        public RoomView StartRoom;
        public RoomView[] SandEnemyRooms;
        public RoomView[] WaterEnemyRooms;
        public RoomView[] DarkEnemyRooms;
        public RoomView[] BossRooms;
    }
}