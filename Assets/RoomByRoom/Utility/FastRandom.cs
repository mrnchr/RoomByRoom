using System;

namespace RoomByRoom.Utility
{
    public static class FastRandom
    {
        public static RaceType GetRandomEnemyRace()
        {
            return (RaceType)UnityEngine.Random.Range(1, Enum.GetNames(typeof(RaceType)).Length);
        }
    }
}