using System;

using UnityEngine;
using Rand = UnityEngine.Random;

namespace RoomByRoom.Utility
{
    public static class FastRandom
    {
        public static RaceType EnemyRace => (RaceType)Rand.Range(1, Utils.GetEnumLength<RaceType>());

        public static UnitType EnemyType => (UnitType)Rand.Range(1, Utils.GetEnumLength<UnitType>() - 1);

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
            return GetRandomFunctionValue(min, max, roomCount);
        }

        public static int GetArmorProtection(ArmorType type, int roomCount)
        {
            float min = 1f, max = 2f;
            switch (type)
            {
                case ArmorType.Boots: min = 1f; max = 1.5f;
                    break;
                case ArmorType.BreastPlate: min = 1.5f; max = 2f;
                    break;
                case ArmorType.Gloves: min = 1f; max = 1.5f;
                    break;
                case ArmorType.Helmet: min = 1f; max = 2f;
                    break;
                case ArmorType.Leggings: min = 1.25f; max = 1.75f;
                    break;
            }

            return GetRandomFunctionValue(min, max, roomCount);
        }

        public static int GetPhysicalDamage(WeaponType type, int roomCount)
        {
            float min = 1f, max = 2f;
            switch (type)
            {
                case WeaponType.None: min = 1f / 2; max = 1.5f / 2;
                    break;
                case WeaponType.Bow: min = 1.25f; max = 1.5f;
                    break;
                case WeaponType.OneHand: min = 1.5f; max = 1.75f;
                    break;
                case WeaponType.TwoHand: min = 1.75f; max = 2f;
                    break;
            }

            return GetRandomFunctionValue(min, max, roomCount);
        }

        private static int GetRandomFunctionValue(float min, float max, int roomCount)
        {
            return Mathf.CeilToInt(Rand.Range(min, max) * roomCount * 10);
        }

        public static int GetEnemyRoom(int enemyRoomCount) => Rand.Range(0, enemyRoomCount);
    }
}