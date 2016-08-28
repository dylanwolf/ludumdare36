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
    LightGoal = 7,
    Mirror = 8,
    GearGoal = 9,
    PowerGoal = 10,
    Door = 11,
    LightSwitch = 12
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