using System;

namespace RoomByRoom.Utility
{
    public static class FastRandom
    {
        public static RaceType GetEnemyRace()
        {
            return (RaceType)UnityEngine.Random.Range(1, Enum.GetNames(typeof(RaceType)).Length);
        }

        public static UnitType GetEnemyType()
        {
            return (UnitType)UnityEngine.Random.Range(1, Enum.GetNames(typeof(UnitType)).Length - 1);
        }
    }
}