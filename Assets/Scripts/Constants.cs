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
        Tree = 0,
        Root = 1,
        Stone = 2,
        Fire = 3,
        Water = 4,
        InterestPlace = 5,
        WatchTower = 6,
        Medvedka = 7,
        MedvedkaNest = 8,
        StoneTrigger = 9,
        FireTrigger = 10
    }
}