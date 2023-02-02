using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Constants
{
    public class Constants
    {
        public const float MAP_CELL_SIZE = 10f;
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
}