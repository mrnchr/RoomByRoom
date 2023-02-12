using System;

namespace RoomByRoom
{
    [Serializable]
    public struct RoomInfo
    {
        public RoomType Type;
    }

    public enum RoomType
    {
        Start,
        Enemy,
        Boss
    }
}