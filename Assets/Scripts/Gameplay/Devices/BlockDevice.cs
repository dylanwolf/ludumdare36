using UnityEngine;
using System.Collections;

public class BlockDevice : GameDevice {

    void Awake()
    {
        CanDelete = false;
    }

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
