using System;

namespace RoomByRoom
{
    [Serializable]
    public struct BoundComponent<T>
    where T : struct
    {
        public int BoundEntity;
        public T ComponentInfo;
    }
}