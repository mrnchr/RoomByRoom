using UnityEngine;

namespace RoomByRoom
{
    [CreateAssetMenu(menuName = "RoomByRoom/Data/SavedData")]
    public class SavedData : ScriptableObject
    {
        public RaceInfo RoomRace;
        public RoomInfo RoomType;
        public RaceInfo PlayerRace;
        public Alive PlayerHP;
    }
}