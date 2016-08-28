﻿using UnityEngine;
using System.Collections;
using System;

public class LightGoalDevice : GameDevice {

    public override bool CanDelete { get { return false; } }

    protected override bool CanLight
    {
        get
        {
            return true;
        }
    }

    public override bool IsWinCondition { get { return true; } }

    bool win = false;

    public override bool HasSetWinCondition
    {
        get { return win; }
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Disable()
    {
        base.Disable();
    }

    protected override void ResetTickStateInternal()
    {
        win = false;
    }

    protected override void TickInternal()
    {
        
    }

    protected override void CleanupAfterTickInternal()
    {
        
    }

    protected override void ApplyLightInternal(GameDevice device)
    {
        base.ApplyLightInternal(device);
        win = true;
    }
}