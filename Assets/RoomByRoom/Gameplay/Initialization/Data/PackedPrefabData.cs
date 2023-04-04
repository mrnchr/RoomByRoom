using System;
using System.Collections.Generic;

namespace RoomByRoom
{
    public class PackedPrefabData
    {
        public PrefabData Prefabs;

        public PackedPrefabData(PrefabData prefabs)
        {
            Prefabs = prefabs;
        }

        public ItemView GetItem(ItemType item, int type, int index = -1)
        {
            return index == -1
                ? Prefabs.PlayerHand
                : GetItems(item, type)[index];
        }

        public ItemView[] GetItems(ItemType item, int type)
        {
            switch(item)
            {
                case ItemType.Armor : return GetArmors((ArmorType)type);
                case ItemType.Artifact : return Prefabs.Artifacts;
                case ItemType.Weapon : return GetWeapons((WeaponType)type);

                default: throw new ArgumentOutOfRangeException();
            }
        }

        private ItemView[] GetArmors(ArmorType type)
        {
            switch(type)
            {
                case ArmorType.Boots : return Prefabs.Boots;
                case ArmorType.BreastPlate : return Prefabs.Breastplates;
                case ArmorType.Gloves : return Prefabs.Gloves;
                case ArmorType.Helmet : return Prefabs.Helmets;
                case ArmorType.Leggings : return Prefabs.Leggings;
                case ArmorType.Shield : return Prefabs.Shields;

                default: throw new ArgumentOutOfRangeException();
            }
        }

        private ItemView[] GetWeapons(WeaponType type)
        {
            switch(type)
            {
                case WeaponType.Bow : return Prefabs.Bows;
                case WeaponType.OneHand : return Prefabs.OneHandWeapons;
                case WeaponType.TwoHand : return Prefabs.TwoHandsWeapons;

                default: throw new ArgumentOutOfRangeException();
            }
        }


        public UnitView[] GetEnemies(RaceType race)
        {
            switch (race)
            {
                case RaceType.Sand: return Prefabs.SandEnemyUnits;
                case RaceType.Water: return Prefabs.WaterEnemyUnits;
                case RaceType.Dark: return Prefabs.DarkEnemyUnits;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
