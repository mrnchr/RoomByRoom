using System;
using UnityEngine.Serialization;

namespace RoomByRoom
{
    [Serializable]
    public struct UnitEntity
    {
        // TODO: remove!!! Health is calculated randomly
        public Health Health;
        [FormerlySerializedAs("Moving")] public Movable Movable;
    }
}