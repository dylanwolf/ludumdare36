using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEngine : MonoBehaviour {

    public static GameEngine Current;
    //public static Vector2 TileSize = new Vector2(2.56f, 2.1f);
    public static Vector2 TileSize = new Vector2(2.76f, 2.3f);

    void Awake()
    {
        Current = this;
    }

    void Start()
    {
        LoadCurrentLevel();
    }

    public void LoadCurrentLevel()
    {
        GameState.Mode = GameMode.Playing;
        ToolSelector.Current.SetLevelName(GameState.LevelIndex, GameState.LEVELS[GameState.LevelIndex].Name);
        SetupLevel(GameState.LEVELS[GameState.LevelIndex].Board, GameState.LEVELS[GameState.LevelIndex].Switches);
        LoadParts(GameState.LEVELS[GameState.LevelIndex].Parts);
        ToolSelector.Current.SetSlots();
    }

    public static LevelTile[,] DEMO_LEVEL = new LevelTile[,]
    {
        { LevelTile.Empty, LevelTile.Empty, LevelTile.Block, LevelTile.LightGoal, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
        { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
        { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
        { LevelTile.Engine, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
    };

    #region Setup
    public void SetupLevel(LevelTile[,] level, GameState.SwitchAssociation[] switches)
    {
        ClearOldLevel();
        GameState.Tiles = new Tile[level.GetLength(0), level.GetLength(1)];
        GameState.Devices = new GameDevice[level.GetLength(0), level.GetLength(1)];
        BuildLevel(level, switches);
    }

    void LoadParts(Dictionary<string, int> parts)
    {
        GameState.Parts.Clear();
        foreach (string key in parts.Keys)
            GameState.Parts.Add(key, parts[key]);
    }

    void ClearOldLevel()
    {
        if (GameState.Tiles != null)
        {
            int height = GameState.Tiles.GetLength(0);
            int width = GameState.Tiles.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (GameState.Tiles[y, x] != null)
                    {
                        ObjectPools.Despawn(GameState.Tiles[y, x]);
                        GameState.Tiles[y, x] = null;
                    }
                    if (GameState.Devices[y, x] != null)
                    {
                        ObjectPools.Despawn(GameState.Devices[y, x]);
                        GameState.Devices[y, x] = null;
                    }
                }
            }
        }
    }

    const string TILE_POOL = "Tiles";
    public const string ENGINE_POOL = "Engines";
    public const string GEAR_POOL = "Gears";
    public const string BLOCK_POOL = "Blocks";
    public const string GENERATOR_POOL = "Generators";
    public const string REPEATER_POOL = "Repeaters";
    public const string LASER_POOL = "Lasers";
    public const string LIGHT_GOAL_POOL = "LightGoals";
    public const string MIRROR_POOL = "Mirrors";
    public const string GEAR_GOAL_POOL = "GearGoals";
    public const string POWER_GOAL_POOL = "PowerGoals";
    public const string DOOR_POOL = "Doors";
    public const string LIGHT_SWITCH_POOL = "LightSwitches";

    void BuildLevel(LevelTile[,] level, GameState.SwitchAssociation[] switches)
    {
        GameState.WinConditions.Clear();
        GameState.LevelHeight = GameState.Tiles.GetLength(0);
        GameState.LevelWidth = GameState.Tiles.GetLength(1);

        Vector2 offset = new Vector2(
                -(GameState.LevelWidth / 2.0f) * TileSize.x,
                (GameState.LevelHeight / 2.0f) * TileSize.y
            );

        for (int y = 0; y < GameState.LevelHeight; y++)
        {
            for (int x = 0; x < GameState.LevelWidth; x++)
            {
                Vector3 pos = new Vector3(offset.x + (x * TileSize.x), offset.y - (y * TileSize.y));
                GameState.Tiles[y, x] = ObjectPools.Spawn<Tile>(TILE_POOL, pos, null);
                GameState.Tiles[y, x].TileX = x;
                GameState.Tiles[y, x].TileY = y;

                switch (level[y,x])
                {
                    case LevelTile.Engine:
                        CreateDevice(ENGINE_POOL, x, y);
                        break;
                    case LevelTile.Block:
                        CreateDevice(BLOCK_POOL, x, y);
                        break;
                    case LevelTile.Gear:
                        CreateDevice(GEAR_POOL, x, y);
                        break;
                    case LevelTile.Generator:
                        CreateDevice(GENERATOR_POOL, x, y);
                        break;
                    case LevelTile.Repeater:
                        CreateDevice(REPEATER_POOL, x, y);
                        break;
                    case LevelTile.Laser:
                        CreateDevice(LASER_POOL, x, y);
                        break;
                    case LevelTile.LightGoal:
                        CreateDevice(LIGHT_GOAL_POOL, x, y);
                        break;
                    case LevelTile.Mirror:
                        CreateDevice(MIRROR_POOL, x, y);
                        break;
                    case LevelTile.GearGoal:
                        CreateDevice(GEAR_GOAL_POOL, x, y);
                        break;
                    case LevelTile.PowerGoal:
                        CreateDevice(POWER_GOAL_POOL, x, y);
                        break;
                    case LevelTile.LightSwitch:
                        CreateDevice(LIGHT_SWITCH_POOL, x, y);
                        break;
                    case LevelTile.Door:
                        CreateDevice(DOOR_POOL, x, y);
                        break;
                }
            }
        }

        if (switches != null)
        {
            for (int i = 0; i < switches.Length; i++)
            {
                GameState.Devices[switches[i].Switch[0], switches[i].Switch[1]].SetSwitchTarget(
                        switches[i].Target[1], switches[i].Target[0]
                    );
            }
        }
    }

    Tile tile;
    GameDevice dev;
    Vector3 tmp;
    public void CreateDevice(string poolName, int x, int y)
    {
        tile = GameState.Tiles[y, x];
        tmp = tile.transform.position;
        tmp.z += (y * 0.01f);
        dev = ObjectPools.Spawn<GameDevice>(poolName, tmp, tile.transform);
        GameState.Devices[y, x] = dev;
        dev.TileX = x;
        dev.TileY = y;

        if (dev.IsWinCondition)
            GameState.WinConditions.Add(dev);
    }
    #endregion

    #region Simulation
    void Update()
    {
        if (GameState.Mode == GameMode.Playing || GameState.Mode == GameMode.Won)
            Tick();
    }

    public void Tick()
    {
        for (int y = 0; y < GameState.LevelHeight; y++)
        {
            for (int x = 0; x < GameState.LevelWidth; x++)
            {
                if (GameState.Devices[y, x] != null)
                    GameState.Devices[y, x].ResetTickState();
            }
        }

        for (int y = 0; y < GameState.LevelHeight; y++)
        {
            for (int x = 0; x < GameState.LevelWidth; x++)
            {
                if (GameState.Devices[y, x] != null)
                {
                    if (GameState.Devices[y, x] != null)
                        GameState.Devices[y, x].Tick();
                }
            }
        }

        for (int y = 0; y < GameState.LevelHeight; y++)
        {
            for (int x = 0; x < GameState.LevelWidth; x++)
            {
                if (GameState.Devices[y, x] != null)
                    GameState.Devices[y, x].CleanupAfterTick();
            }
        }

        if (GameState.Mode == GameMode.Playing && GameState.WinConditions.Count > 0)
        {
            bool hasWon = true;
            for (int i = 0; i < GameState.WinConditions.Count; i++)
            {
                if (!GameState.WinConditions[i].HasSetWinCondition)
                {
                    hasWon = false;
                    break;
                }
            }

            if (hasWon)
                WonGame();
        }
    }

    public void Crank(GameDevice crankingDevice, int x, int y)
    {
        if (y < 0 || x < 0 || y >= GameState.LevelHeight || x >= GameState.LevelWidth || GameState.Devices[y,x] == null)
            return;

        GameState.Devices[y, x].ApplyCrank(crankingDevice);
    }

    public void Power(GameDevice poweringDevice, int x, int y)
    {
        if (y < 0 || x < 0 || y >= GameState.LevelHeight || x >= GameState.LevelWidth || GameState.Devices[y, x] == null)
            return;

        GameState.Devices[y, x].ApplyPower(poweringDevice);
    }

    public void Light(GameDevice lightingDevice, int x, int y)
    {
        if (y < 0 || x < 0 || y >= GameState.LevelHeight || x >= GameState.LevelWidth || GameState.Devices[y, x] == null)
            return;

        GameState.Devices[y, x].ApplyLight(lightingDevice);
    }

    public void Switch(GameDevice switchingDevice, int x, int y)
    {
        if (y < 0 || x < 0 || y >= GameState.LevelHeight || x >= GameState.LevelWidth || GameState.Devices[y, x] == null)
            return;

        GameState.Devices[y, x].ApplySwitch(switchingDevice);
    }
    #endregion

    #region Editor
    public void SelectTool(string poolName)
    {
        GameState.EditorSelection = poolName;
    }

    const string SOUND_DELETE = "DeleteDevice";
    const string SOUND_ADD = "PlaceDevice";

    public void ApplyTool(int x, int y)
    {
        if (GameState.Mode != GameMode.Playing)
            return;

        if (GameState.EditorSelection == GameState.EDITOR_DELETE)
        {
            if (GameState.Devices[y,x] != null && GameState.Devices[y,x].CanDelete)
            {
                GameState.Parts[GameState.Devices[y, x].PartName]++;
                ObjectPools.Despawn(GameState.Devices[y,x]);
                GameState.Devices[y,x] = null;
                ToolSelector.Current.UpdateCounts();
                SoundBoard.Play(SOUND_DELETE);
            }
        }
        else if (GameState.EditorSelection != null)
        {
            if (GameState.Devices[y, x] == null && GameState.Parts.ContainsKey(GameState.EditorSelection) && GameState.Parts[GameState.EditorSelection] > 0)
            {
                CreateDevice(GameState.EditorSelection, x, y);
                GameState.Devices[y, x].PartName = GameState.EditorSelection;
                GameState.Parts[GameState.EditorSelection]--;
                ToolSelector.Current.UpdateCounts();
                SoundBoard.Play(SOUND_ADD);
            }
        }
    }
    #endregion

    #region WinState
    public void WonGame()
    {
        GameState.Mode = GameMode.Won;
        FinishedLevelWindow.Current.Show();
        if (GameState.LevelIndex < GameState.LEVELS.Length - 1)
        {
            GameState.LevelIndex++;
            FinishedLevelWindow.Current.ShowNextLevel(true);
        }
        else
        {
            FinishedLevelWindow.Current.ShowNextLevel(false);
        }
    }
    #endregion
}
