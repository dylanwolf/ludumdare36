using UnityEngine;
using System.Collections;
using System;

public class LightSwitchDevice : GameDevice {

    public override bool CanDelete
    {
        get
        {
            return false;
        }
    }

    protected override bool CanLight
    {
        get
        {
            return true;
        }
    }

    protected override void ResetTickStateInternal()
    {
        
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
        DoSwitch();
    }
}
