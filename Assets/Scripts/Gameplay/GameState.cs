using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameState {

    public static GameMode Mode;

    public static string EditorSelection;
    public const string EDITOR_DELETE = "DELETE MODE";

    public static int LevelWidth;
    public static int LevelHeight;

    public static Tile[,] Tiles;
    public static GameDevice[,] Devices;
    public static List<GameDevice> WinConditions = new List<GameDevice>();
    public static Dictionary<string, int> Parts = new Dictionary<string, int>();

    public class Level
    {
        public string Name;
        public LevelTile[,] Board;
        public Dictionary<string, int> Parts = new Dictionary<string, int>();
        public SwitchAssociation[] Switches;
    }

    public class SwitchAssociation
    {
        public int[] Switch;
        public int[] Target;
    }


    public static int LevelIndex = 0;

    public static Level[] LEVELS = new Level[]
    {
        // Level 1
        new Level()
        {
            Name = "Gears",
            Board = new LevelTile[,]
            {
                { LevelTile.Engine, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.GearGoal }
            },
            Parts = new Dictionary<string, int>()
            {
                {  GameEngine.GEAR_POOL, 3 }
            },
        },

        // Level 2
        new Level()
        {
            Name = "Power",
            Board = new LevelTile[,]
            {
                { LevelTile.Engine, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.PowerGoal }
            },
            Parts = new Dictionary<string, int>()
            {
                {  GameEngine.GEAR_POOL, 1 },
                {  GameEngine.GENERATOR_POOL, 1 },
                {  GameEngine.REPEATER_POOL, 1 },
            },
        },

        // Level 3
        new Level()
        {
            Name = "Light",
            Board = new LevelTile[,]
            {
                { LevelTile.Engine, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.LightGoal }
            },
            Parts = new Dictionary<string, int>()
            {
                {  GameEngine.GENERATOR_POOL, 1 },
                {  GameEngine.LASER_POOL, 1 },
            },
        },

        // Level 4
        new Level()
        {
            Name = "All The Things",
            Board = new LevelTile[,]
            {
                { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.LightGoal },
                { LevelTile.Engine, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
                { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.LightGoal }
            },
            Parts = new Dictionary<string, int>()
            {
                {  GameEngine.GENERATOR_POOL, 1 },
                {  GameEngine.LASER_POOL, 2 },
                {  GameEngine.REPEATER_POOL, 2 },
            },
        },

        // Level 5
        new Level()
        {
            Name = "Split",
            Board = new LevelTile[,]
            {
                { LevelTile.LightGoal, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.LightGoal },
                { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
                { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
                { LevelTile.Empty, LevelTile.Empty, LevelTile.Engine, LevelTile.Empty, LevelTile.Empty }
            },
            Parts = new Dictionary<string, int>()
            {
                {  GameEngine.GENERATOR_POOL, 1 },
                {  GameEngine.LASER_POOL, 1 },
                { GameEngine.MIRROR_POOL, 1 }, 
            },
        },

        // Level 6
        new Level()
        {
            Name = "Workaround",
            Board = new LevelTile[,]
            {
                { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
                { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Block, LevelTile.Empty },
                { LevelTile.Engine, LevelTile.Empty, LevelTile.Empty, LevelTile.Block, LevelTile.LightGoal },
                { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Block, LevelTile.Empty },
                { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
            },
            Parts = new Dictionary<string, int>()
            {
                {  GameEngine.GEAR_POOL, 2 },
                {  GameEngine.GENERATOR_POOL, 1 },
                {  GameEngine.LASER_POOL, 1 },
                { GameEngine.REPEATER_POOL, 3 },
            },
        },

        // Level 7
        new Level()
        {
            Name = "Collect All Three",
            Board = new LevelTile[,]
            {
                { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
                { LevelTile.Empty, LevelTile.Empty, LevelTile.GearGoal, LevelTile.Empty, LevelTile.Empty },
                { LevelTile.Engine, LevelTile.Empty, LevelTile.Empty, LevelTile.Block, LevelTile.LightGoal },
                { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.PowerGoal, LevelTile.Empty },
                { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
            },
            Parts = new Dictionary<string, int>()
            {
                {  GameEngine.GEAR_POOL, 2 },
                {  GameEngine.GENERATOR_POOL, 1 },
                {  GameEngine.LASER_POOL, 1 },
                { GameEngine.REPEATER_POOL, 3 },
            },
        },

        // Level 8
        new Level()
        {
            Name = "Open The Way",
            Board = new LevelTile[,]
            {
                { LevelTile.Empty, LevelTile.Empty, LevelTile.LightSwitch, LevelTile.Empty, LevelTile.Empty },
                { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
                { LevelTile.Engine, LevelTile.Empty, LevelTile.Empty, LevelTile.Door, LevelTile.LightGoal },
                { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
                { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
            },
            Parts = new Dictionary<string, int>()
            {
                {  GameEngine.GENERATOR_POOL, 1 },
                {  GameEngine.LASER_POOL, 1 },
                { GameEngine.REPEATER_POOL, 2 },
            },
            Switches = new SwitchAssociation[]
            {
                new SwitchAssociation() { Switch = new int[] { 0, 2 }, Target = new int[] { 2, 3 } }
            }
        },
    };
}
