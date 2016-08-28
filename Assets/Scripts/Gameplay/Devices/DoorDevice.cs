using UnityEngine;
using System.Collections;
using System;

public class DoorDevice : GameDevice {

    public override bool CanDelete
    {
        get
        {
            return false;
        }
    }

    protected override bool CanSwitch
    {
        get
        {
            return true;
        }
    }

    public override bool BlocksLaser
    {
        get
        {
            return !switched;
        }
    }

    bool switched = false;

    protected override void ResetTickStateInternal()
    {
        switched = false;
    }

    protected override void TickInternal()
    {
        
    }

    protected override void CleanupAfterTickInternal()
    {
        if (_r != null)
            _r.enabled = !switched;
    }

    protected override void ApplySwitchInternal(GameDevice device)
    {
        switched = true;
    }

}
