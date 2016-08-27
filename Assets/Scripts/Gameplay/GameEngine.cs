using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

    public static GameEngine Current;
    public static Vector2 TileSize = new Vector2(2.56f, 1.92f);

	void Awake()
    {
        Current = this;
    }

    void Start()
    {
        SetupLevel(DEMO_LEVEL);
    }

    public static LevelTile[,] DEMO_LEVEL = new LevelTile[,]
    {
        { LevelTile.Empty, LevelTile.Empty, LevelTile.Block, LevelTile.LightGoal, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
        { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
        { LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
        { LevelTile.Engine, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty, LevelTile.Empty },
    };

    #region Setup
    public void SetupLevel(LevelTile[,] level)
    {
        ScreenScroll2D.Current.Reset();
        ClearOldLevel();
        GameState.Tiles = new Tile[level.GetLength(0), level.GetLength(1)];
        GameState.Devices = new GameDevice[level.GetLength(0), level.GetLength(1)];
        BuildLevel(level);
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

    void BuildLevel(LevelTile[,] level)
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
                }
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
        if (GameState.Mode == GameMode.Playing)
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

        if (GameState.WinConditions.Count > 0)
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
    #endregion

    #region Editor
    public void SelectTool(string poolName)
    {
        GameState.EditorSelection = poolName;
    }

    public void ApplyTool(int x, int y)
    {
        if (GameState.EditorSelection == GameState.EDITOR_DELETE)
        {
            if (GameState.Devices[y,x] != null && GameState.Devices[y,x].CanDelete)
            {
                ObjectPools.Despawn(GameState.Devices[y,x]);
                GameState.Devices[y,x] = null;
            }
        }
        else if (GameState.EditorSelection != null)
        {
            if (GameState.Devices[y,x] == null)
                CreateDevice(GameState.EditorSelection, x, y);
        }
    }
    #endregion

    #region WinState
    public void WonGame()
    {
        GameState.Mode = GameMode.Won;
        Debug.Log("Won game!");
    }
    #endregion
}
