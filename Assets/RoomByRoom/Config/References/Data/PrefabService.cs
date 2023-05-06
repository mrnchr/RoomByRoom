using System;

namespace RoomByRoom
{
  public class PrefabService
  {
    public readonly PrefabData Prefabs;

    public PrefabService(PrefabData prefabs)
    {
      Prefabs = prefabs;
    }

    public ItemView GetItem(ItemType item, int type, int index = -1) =>
      index == -1
        ? Prefabs.PlayerHand
        : GetItems(item, type)[index];

    public ItemView[] GetItems(ItemType item, int type) =>
      item switch
      {
        ItemType.Armor    => GetArmors((ArmorType)type),
        ItemType.Artifact => Prefabs.Artifacts,
        ItemType.Weapon   => GetWeapons((WeaponType)type),
        _                 => throw new ArgumentOutOfRangeException()
      };

    private ItemView[] GetArmors(ArmorType type) =>
      type switch
      {
        ArmorType.Boots       => Prefabs.Boots,
        ArmorType.BreastPlate => Prefabs.Breastplates,
        ArmorType.Gloves      => Prefabs.Gloves,
        ArmorType.Helmet      => Prefabs.Helmets,
        ArmorType.Leggings    => Prefabs.Leggings,
        ArmorType.Shield      => Prefabs.Shields,
        _                     => throw new ArgumentOutOfRangeException()
      };

    private ItemView[] GetWeapons(WeaponType type) =>
      type switch
      {
        WeaponType.Bow      => Prefabs.Bows,
        WeaponType.OneHand  => Prefabs.OneHands,
        WeaponType.TwoHands => Prefabs.TwoHands,
        _                   => throw new ArgumentOutOfRangeException()
      };

    public UnitView[] GetEnemies(RaceType race) =>
      race switch
      {
        RaceType.Sand  => Prefabs.SandEnemyUnits,
        RaceType.Water => Prefabs.WaterEnemyUnits,
        RaceType.Dark  => Prefabs.DarkEnemyUnits,
        _              => throw new ArgumentOutOfRangeException()
      };
  }
}