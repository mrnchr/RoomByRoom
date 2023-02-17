using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomByRoom
{
    public class PackedPrefabData
    {
        public PrefabData Prefabs;
        public Dictionary<RaceType, RoomView[]> EnemyRooms;
        public Dictionary<RaceType, UnitView[]> EnemyUnits;

        public PackedPrefabData(PrefabData prefabs)
        {
            Prefabs = prefabs;
            PackEnemyRooms();
            PackEnemyUnits();
        }

        private void PackEnemyUnits()
        {
            EnemyUnits = new Dictionary<RaceType, UnitView[]>();

            EnemyUnits[RaceType.Sand] = Prefabs.SandEnemyUnits;
            EnemyUnits[RaceType.Water] = Prefabs.WaterEnemyUnits;
            EnemyUnits[RaceType.Dark] = Prefabs.DarkEnemyUnits;
        }

        private void PackEnemyRooms()
        {
            EnemyRooms = new Dictionary<RaceType, RoomView[]>();

            EnemyRooms[RaceType.Sand] = Prefabs.SandEnemyRooms;
            EnemyRooms[RaceType.Water] = Prefabs.WaterEnemyRooms;
            EnemyRooms[RaceType.Dark] = Prefabs.DarkEnemyRooms;
        }
    }
}
