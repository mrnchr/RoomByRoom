using System;

namespace RoomByRoom
{
    [Serializable]
    public struct BoundComponent<T>
    {
        public int BoundEntity;
        public T ComponentInfo;
    }
}