using System;

using Rand = UnityEngine.Random;

namespace RoomByRoom.Utility
{
    public static class FastRandom
    {
        public static RaceType EnemyRace => (RaceType)Rand.Range(1, GetEnumLength<RaceType>());

        public static UnitType EnemyType => (UnitType)Rand.Range(1, GetEnumLength<UnitType>() - 1);

        public static int GetUnitHP(int roomCount, UnitType type)
        {
            float min = 1f, max = 2f;
            switch (type)
            {
                case UnitType.Baby: min = 1f; max = 1.5f;
                    break;
                case UnitType.Flying: min = 1f; max = 1.75f;
                    break;
                case UnitType.Humanoid: min = 1.25f; max = 1.75f;
                    break;
                case UnitType.Giant: min = 1.5f; max = 2f;
                    break;
                case UnitType.Boss: min = 2f; max = 2f;
                    break;
            }
            return (int)(Rand.Range(min, max) * roomCount * 10);
        }

        public static int GetEnemyRoom(int enemyRoomCount) => Rand.Range(0, enemyRoomCount);

        private static int GetEnumLength<T>() 
        where T : Enum => Enum.GetNames(typeof(T)).Length;
    }
}