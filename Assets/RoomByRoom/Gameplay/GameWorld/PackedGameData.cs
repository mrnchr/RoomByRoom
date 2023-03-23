using System;
using System.Collections.Generic;

namespace RoomByRoom
{
    public class PackedGameData
    {
        public GameData GameData;

        public PackedGameData(GameData gameData)
        {
            GameData = gameData;
        }

        public UnitEntity GetUnitEntity(UnitType unit, RaceType race)
        {
            switch(unit)
            {
                case UnitType.Baby: return GameData.Baby.Unit;
                case UnitType.Flying: return GameData.Flying.Unit;
                case UnitType.Humanoid: return GameData.Humanoid.Unit;
                case UnitType.Giant: return GameData.Giant.Unit;
                case UnitType.Boss: return GetBossEntity<GroundUnitEntity>(race).Unit;

                default: throw new ArgumentException($"Unit with type {unit} doesn't exist");
            }
        }

        public T GetUnitTypeEntity<T>(UnitType unit, RaceType race)
        where T : class, new()
        {
            T unitEntity = new T();
            if(unitEntity is GroundUnitEntity groundUnit)
            {
                switch(unit)
                {
                    case UnitType.Baby: groundUnit = GameData.Baby;
                        break;
                    case UnitType.Humanoid: groundUnit = GameData.Humanoid;
                        break;
                    case UnitType.Giant: groundUnit = GameData.Giant;
                        break;
                    case UnitType.Boss: groundUnit = GetBossEntity<GroundUnitEntity>(race);
                        break;

                    default: throw new ArgumentException($"Unit with type {unit} doesn't exist");
                }

                unitEntity = groundUnit as T;
            }
            else if(unitEntity is FlyingUnitEntity flyingUnit)
            {
                unitEntity = GameData.Flying as T;
            }
            else 
            {
                throw new ArgumentException($"Entity with type {nameof(T)} doesn't exist in game data");
            }

            return unitEntity;
        }

        private T GetBossEntity<T>(RaceType race)
        where T : class, new()
        {
            T bossEntity = new T();
            if(bossEntity is GroundUnitEntity groundUnit)
            {
                switch(race)
                {
                    case RaceType.Sand: groundUnit = GameData.SandBoss;
                        break;
                    case RaceType.Water: groundUnit = GameData.WaterBoss;
                        break;
                    case RaceType.Dark: groundUnit = GameData.DarkBoss;
                        break;

                    default: throw new ArgumentException($"Boss with race {race} doesn't exist");
                }

                bossEntity = groundUnit as T;
            }

            return bossEntity;
        }


    }
}
