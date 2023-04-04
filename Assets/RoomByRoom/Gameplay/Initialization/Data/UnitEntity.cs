using System;

namespace RoomByRoom
{
    [Serializable]
    public struct UnitEntity
    {
        // TODO: remove!!! Health is calculated randomly
        public Health Health;
        public Moving Moving;
    }
}