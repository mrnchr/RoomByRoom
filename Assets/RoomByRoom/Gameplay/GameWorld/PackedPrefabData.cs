using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomByRoom
{
    public class PackedPrefabData
    {
        public PrefabData Prefabs;
        public Dictionary<RaceType, UnitView[]> EnemyUnits;

        public ItemView this[ItemType item, int type, int index = -1]
        {
            get
            {
                switch(item)
                {
                    case ItemType.Armor : return this[(ArmorType)type, index];
                    case ItemType.Artifact : return Prefabs.Artifacts[index];
                    case ItemType.Weapon : return this[(WeaponType)type, index];

                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        private ItemView this[ArmorType type, int index]
        {
            get
            {
                switch(type)
                {
                    case ArmorType.Boots : return Prefabs.Boots[index];
                    case ArmorType.BreastPlate : return Prefabs.Breastplates[index];
                    case ArmorType.Gloves : return Prefabs.Gloves[index];
                    case ArmorType.Helmet : return Prefabs.Helmets[index];
                    case ArmorType.Leggings : return Prefabs.Leggings[index];
                    case ArmorType.Shield : return Prefabs.Shields[index];

                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        private ItemView this[WeaponType type, int index = -1]
        {
            get
            {
                switch(type)
                {
                    case WeaponType.Bow : return Prefabs.Bows[index];
                    case WeaponType.None : return Prefabs.PlayerHand;
                    case WeaponType.OneHand : return Prefabs.OneHandWeapons[index];
                    case WeaponType.TwoHand : return Prefabs.TwoHandsWeapons[index];

                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        public PackedPrefabData(PrefabData prefabs)
        {
            Prefabs = prefabs;
            PackEnemyUnits();
        }

        private void PackEnemyUnits()
        {
            EnemyUnits = new Dictionary<RaceType, UnitView[]>();

            EnemyUnits[RaceType.Sand] = Prefabs.SandEnemyUnits;
            EnemyUnits[RaceType.Water] = Prefabs.WaterEnemyUnits;
            EnemyUnits[RaceType.Dark] = Prefabs.DarkEnemyUnits;
        }
    }
}
