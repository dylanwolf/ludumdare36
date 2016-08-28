using UnityEngine;
using System.Collections;
using System;

public class GearDevice : GameDevice {

    protected override bool CanCrank
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
            _r.sprite = (CrankedBy.Count > 0) ? ActiveSprite : InactiveSprite;
    }

    protected override void ApplyCrankInternal(GameDevice device)
    {
        base.ApplyCrankInternal(device);
        GameEngine.Current.Crank(this, TileX - 1, TileY);
        GameEngine.Current.Crank(this, TileX + 1, TileY);
        GameEngine.Current.Crank(this, TileX, TileY - 1);
        GameEngine.Current.Crank(this, TileX, TileY + 1);

        if (_r != null)
            _r.sprite = (CrankedBy.Count > 0) ? ActiveSprite : InactiveSprite;
    }
}
