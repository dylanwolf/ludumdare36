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

    public Sprite ActiveSprite;
    public Sprite InactiveSprite;

    public SpriteRenderer SwitchSprite;

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
        if (SwitchSprite != null)
            SwitchSprite.sprite = LitBy.Count > 0 ? ActiveSprite : InactiveSprite;
    }

    protected override void ApplyLightInternal(GameDevice device)
    {
        base.ApplyLightInternal(device);
        DoSwitch();
    }
}
