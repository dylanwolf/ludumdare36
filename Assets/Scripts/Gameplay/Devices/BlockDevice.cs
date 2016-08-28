using UnityEngine;
using System.Collections;

public class BlockDevice : GameDevice {

    public override bool CanDelete { get { return false; } }

    protected override void TickInternal()
    {

    }

    protected override void ResetTickStateInternal()
    {

    }

    protected override void CleanupAfterTickInternal()
    {

    }
}
