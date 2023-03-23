using UnityEngine;

namespace RoomByRoom
{
    [CreateAssetMenu(menuName = "RoomByRoom/Data/GameData")]
    public class GameData : ScriptableObject
    {
        public FlyingUnitEntity Flying;
        public GroundUnitEntity Giant;
        public GroundUnitEntity Humanoid;
        public GroundUnitEntity Baby;
        public GroundUnitEntity SandBoss;
        public GroundUnitEntity WaterBoss;
        public GroundUnitEntity DarkBoss;
    }
}