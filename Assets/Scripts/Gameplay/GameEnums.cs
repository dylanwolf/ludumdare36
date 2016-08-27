using UnityEngine;
using System.Collections;

public enum LevelTile
{
    Empty = 0,
    Block = 1,
    Engine = 2,
    Gear = 3,
    Generator = 4,
    Repeater = 5,
    Laser = 6,
    LightGoal = 7
}

public enum DeviceAnimation
{
    Stopped = 0,
    Starting = 1,
    Running = 2,
    Ending = 3
}

public enum GameMode
{
    Playing = 0,
    Won = 1
}