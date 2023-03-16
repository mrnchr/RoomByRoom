using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace RoomByRoom
{
    [Serializable]
    public class InventoryEntity
    {
        public List<BoundComponent<ItemInfo>> Item = new List<BoundComponent<ItemInfo>>();
        public List<BoundComponent<WeaponInfo>> Weapon = new List<BoundComponent<WeaponInfo>>();
        public List<BoundComponent<PhysicalDamage>> PhysDamage = new List<BoundComponent<PhysicalDamage>>();
        public List<BoundComponent<Protection>> Protection = new List<BoundComponent<Protection>>();
        public List<BoundComponent<Equipped>> Equipped = new List<BoundComponent<Equipped>>();
    }
}