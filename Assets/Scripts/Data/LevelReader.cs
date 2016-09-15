using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class LevelReader {

    public static Level[] Load(string filename, out int startingIndex)
    {
        var targetFile = Resources.Load<TextAsset>(filename);
        return Parse(targetFile.text, out startingIndex);
    }

    const string ASTERISK = "*";

    public static Level[] Parse(string text, out int startingIndex)
    {
        startingIndex = 0;
        List<Level> levels = new List<Level>();

        string[] lines = text.Split(NEWLINE, StringSplitOptions.None);
        int index = 0;

        while (index < lines.Length)
        {
            if (String.IsNullOrEmpty(lines[index]))
            {
                index++;
                continue;
            }

            try
            {
                Level l = new Level();
                l.Name = lines[index].Trim();
                if (l.Name.StartsWith(ASTERISK))
                {
                    startingIndex = levels.Count;
                    l.Name = l.Name.Substring(1);
                }
                index++;
                l.Board = ParseBoard(lines[index]);
                index++;
                l.Parts = ParseParts(lines[index]);
                index++;
                if (index < lines.Length)
                    l.Switches = ParseSwitches(lines[index].Trim());
                index++;
                levels.Add(l);
            }
            catch (Exception ex)
            {
                Debug.Log("Error parsing level file on line " + (index + 1).ToString());
                throw ex;
            }
        }


        return levels.ToArray();
    }

    static readonly string[] NEWLINE = new string[] { "\n" };
    static readonly string[] BOARD_ROW_SEPARATOR = new string[] { "||" };
    static readonly string[] BOARD_TILE_SEPARATOR = new string[] { "," };
    static readonly string[] PART_SEPARATOR = new string[] { "||" };
    static readonly string[] PART_QTY_SEPARATOR = new string[] { ":" };
    static readonly string[] SWITCH_SEPARATOR = new string[] { "||" };
    static readonly string[] SWITCH_COORD_SEPARATOR = new string[] { "," };
    static readonly string[] SWITCH_TARGET_SEPARATOR = new string[] { ":" };

    static LevelTile?[,] ParseBoard(string line)
    {

        string[] rows = line.Split(BOARD_ROW_SEPARATOR, StringSplitOptions.None);
        LevelTile?[,] tiles = new LevelTile?[rows.Length, rows[0].Split(BOARD_TILE_SEPARATOR, StringSplitOptions.None).Length];
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            string[] cells = rows[i].Split(BOARD_TILE_SEPARATOR, StringSplitOptions.None);
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                int cellType;
                if (int.TryParse(cells[j], out cellType))
                {
                    tiles[i, j] = (LevelTile)int.Parse(cells[j]);
                }
            }
        }
        return tiles;
    }

    static Dictionary<string, int> ParseParts(string line)
    {
        line = line.Trim();
        Dictionary<string, int> result = new Dictionary<string, int>();

        string[] parts = line.Split(PART_SEPARATOR, StringSplitOptions.None);
        for (int i = 0; i < parts.Length; i++)
        {
            string[] x = parts[i].Split(PART_QTY_SEPARATOR, StringSplitOptions.None);
            result[x[0]] = int.Parse(x[1]);
        }

        return result;
    }

    static SwitchAssociation[] ParseSwitches(string line)
    {
        if (line == null)
            return null;

        line = line.Trim();
        if (String.IsNullOrEmpty(line))
            return null;

        List<SwitchAssociation> result = new List<SwitchAssociation>();

        var switches = line.Split(SWITCH_SEPARATOR, StringSplitOptions.None);
        for (int i = 0; i < switches.Length; i++)
        {
            string[] refs = switches[i].Split(SWITCH_TARGET_SEPARATOR, StringSplitOptions.None);
            string[] switchCoords = refs[0].Split(SWITCH_COORD_SEPARATOR, StringSplitOptions.None);
            string[] targetCoords = refs[1].Split(SWITCH_COORD_SEPARATOR, StringSplitOptions.None);

            result.Add(new SwitchAssociation()
            {
                Switch = new int[] { int.Parse(switchCoords[0]), int.Parse(switchCoords[1]) },
                Target = new int[] { int.Parse(targetCoords[0]), int.Parse(targetCoords[1]) },
            });
        }

        return result.ToArray();
    }
}

public class Level
{
    public string Name;
    public LevelTile?[,] Board;
    public Dictionary<string, int> Parts = new Dictionary<string, int>();
    public SwitchAssociation[] Switches;
}

public class SwitchAssociation
{
    public int[] Switch;
    public int[] Target;
}