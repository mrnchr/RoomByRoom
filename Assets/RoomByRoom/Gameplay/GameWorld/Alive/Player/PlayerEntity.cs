using System;

namespace RoomByRoom
{
    [Serializable]
    public struct PlayerEntity
    {
        public RaceInfo Race;
        public Health Health;
        public Moving Moving;
        public Jumping Jumping;
    }
}