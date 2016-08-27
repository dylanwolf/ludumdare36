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
}
