using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace RoomByRoom
{
  public struct UnitViewRef
  {
    public UnitView Value;
  }

  [Serializable]
  public struct UnitInfo
  {
    public UnitType Type;
  }

  [Serializable]
  public struct RaceInfo
  {
    public RaceType Type;
  }

  [Serializable]
  public struct Inventory
  {
    [SerializeField] public List<EcsPackedEntity> ItemList;
  }

  [Serializable]
  public struct Equipment
  {
    [SerializeField] public List<EcsPackedEntity> ItemList;
  }

  [Serializable]
  public struct Backpack
  {
    [SerializeField] public List<EcsPackedEntity> ItemList;
  }
}