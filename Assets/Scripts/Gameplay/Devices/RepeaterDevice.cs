using UnityEngine;
using System.Collections;

public class RepeaterDevice : GameDevice {

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
        if (_r != null)
            _r.sprite = (PoweredBy.Count > 0) ? ActiveSprite : InactiveSprite;
    }

    protected override void ApplyPowerInternal(GameDevice device)
    {
        base.ApplyCrankInternal(device);
        GameEngine.Current.Power(this, TileX - 1, TileY);
        GameEngine.Current.Power(this, TileX + 1, TileY);
        GameEngine.Current.Power(this, TileX, TileY - 1);
        GameEngine.Current.Power(this, TileX, TileY + 1);
    }
}
