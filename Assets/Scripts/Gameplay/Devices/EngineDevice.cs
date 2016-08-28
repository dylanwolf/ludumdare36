using UnityEngine;
using System.Collections;
using System;

public class EngineDevice : GameDevice {

    public override bool CanDelete { get { return false; } }

    protected override void TickInternal()
    {
        GameEngine.Current.Crank(this, TileX - 1, TileY);
        GameEngine.Current.Crank(this, TileX + 1, TileY);
        GameEngine.Current.Crank(this, TileX, TileY - 1);
        GameEngine.Current.Crank(this, TileX, TileY + 1);
    }

    protected override void ResetTickStateInternal()
    {
        
    }

    protected override void CleanupAfterTickInternal()
    {
        
    }
}
