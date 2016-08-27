using UnityEngine;
using System.Collections;

public class LaserDevice : GameDevice {

    protected override bool CanPower
    {
        get { return true; }
    }

    public Sprite ActiveSprite;
    public Sprite InactiveSprite;

    protected override void ResetTickStateInternal()
    {
    }

    protected override void TickInternal()
    {
    }

    protected override void CleanupAfterTickInternal()
    {
        _r.sprite = (PoweredBy.Count > 0) ? ActiveSprite : InactiveSprite;
    }

    protected override void ApplyPowerInternal(GameDevice device)
    {
        base.ApplyCrankInternal(device);
        // TODO: Generate laser beams
    }
}
