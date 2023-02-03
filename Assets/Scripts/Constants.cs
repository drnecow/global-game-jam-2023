using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Constants
{
    public class Constants
    {
        public const float MAP_CELL_SIZE = 10f;

        public static readonly Vector2[] NEIGHBOURS_1X1 = { new Vector2(0, -1), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, 1) };
        public static readonly Vector2[] NEIGHBOURS_2X2 =
        {
            new Vector2(0, -1),
            new Vector2(1, -1),
            new Vector2(-1, 0),
            new Vector2(2, 0),
            new Vector2(-1, 1),
            new Vector2(2, 1),
            new Vector2(0, 2),
            new Vector2(1, 2)
        };

        public static readonly List<EntityType> INTERACTABLE_ONCE = new List<EntityType> { EntityType.WatchTower, EntityType.RockTrigger, EntityType.FireTrigger,
            EntityType.InterestPlace, EntityType.AstralSalvation };
        public static readonly List<EntityType> INTERACTABLE_MULTIPLE = new List<EntityType> { EntityType.Fire, EntityType.Water };
        public static readonly List<EntityType> DESTRUCTIBLE = new List<EntityType> { EntityType.Rock, EntityType.Medvedka, EntityType.MedvedkaNest };

        public static readonly EntityType[] PERISHABLE_BY_FIRE = { EntityType.Rock, EntityType.WatchTower,
            EntityType.InterestPlace, EntityType.Medvedka, EntityType.MedvedkaNest };
    }

    public enum EntityType
    {
        Tree = 1,
        Root = 2,
        Rock = 3,
        Fire = 4,
        Water = 5,
        InterestPlace = 6,
        WatchTower = 7,
        Medvedka = 8,
        MedvedkaNest = 9,
        RockTrigger = 10,
        FireTrigger = 11,
        AstralSalvation = 12
    }
    public enum RootType
    {
        Single = 1,
        Line = 2,
        Zigzag = 3,
        OutgrowthLine = 4,
        Corner = 5,
        Cross = 6
    }
}