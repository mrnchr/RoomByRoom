using System;
using UnityEngine;

namespace RoomByRoom
{
    [Serializable]
    public struct SpawnPoint
    {
        public Transform UnitSpawn;
        public UnitType UnitType;
    }
}