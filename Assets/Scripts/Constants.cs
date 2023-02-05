using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Constants
{
    public class Constants
    {
        public const float MAP_CELL_SIZE = 10f;
        public static readonly List<Coords> ORIGIN_POINTS = new List<Coords>() { new Coords(1, 6), new Coords(1, 7), new Coords(1, 8) };

        public static readonly List<Vector2> NEIGHBOURS_1X1 = new List<Vector2>() { new Vector2(0, -1), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, 1) };
        public static readonly List<Vector2> NEIGHBOURS_1X1_DIAGONAL = new List<Vector2>()
        {
            new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1, -1),
            new Vector2(-1, 0), new Vector2(1, 0),
            new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1)
        };
        public static readonly List<Vector2> NEIGHBOURS_2X2 = new List<Vector2>()
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

        public static readonly List<EntityType> PERISHABLE_BY_FIRE = new List<EntityType>() { EntityType.Rock, EntityType.WatchTower,
            EntityType.InterestPlace, EntityType.Medvedka, EntityType.MedvedkaNest };


        public static readonly Dictionary<CardType, Action> CARD_ACTIONS = new Dictionary<CardType, Action>()
        {
            [CardType.PlaceSingle] = () => RootBuilder.Instance.PlaceRoot(RootType.Single),
            [CardType.PlaceLine] = () => RootBuilder.Instance.PlaceRoot(RootType.Line),
            [CardType.PlaceZigzag] = () => RootBuilder.Instance.PlaceRoot(RootType.Zigzag),
            [CardType.PlaceOutgrowthLine] = () => RootBuilder.Instance.PlaceRoot(RootType.OutgrowthLine),
            [CardType.PlaceCorner] = () => RootBuilder.Instance.PlaceRoot(RootType.Corner),
            [CardType.PlaceCross] = () => RootBuilder.Instance.PlaceRoot(RootType.Cross),

            [CardType.Poison] = () => RootMap.Instance.DestroyAllEatingMedvedkas(),

            [CardType.MakeFireProof] = () => RootBuilder.Instance.FireProof = true,
            [CardType.MakeHardened] = () => RootBuilder.Instance.Hardened = true,
            [CardType.MakeDrill] = () => RootBuilder.Instance.Drilling = true,

            [CardType.AddFiveSingles] = () => CardSystem.Instance.AddToHand(new List<CardType>() { CardType.PlaceSingle, CardType.PlaceSingle, CardType.PlaceSingle, CardType.PlaceSingle, CardType.PlaceSingle } )
        };
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
    public enum CardType
    {
        PlaceSingle = 1,
        PlaceLine = 2,
        PlaceZigzag = 3,
        PlaceOutgrowthLine = 4,
        PlaceCorner = 5,
        PlaceCross = 6,

        Poison = 7,

        MakeFireProof = 8,
        MakeHardened = 9,
        MakeDrill = 10,

        AddFiveSingles = 11
    }
}