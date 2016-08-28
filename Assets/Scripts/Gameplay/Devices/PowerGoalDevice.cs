using UnityEngine;
using System.Collections;

public class PowerGoalDevice : GameDevice {

    public override bool CanDelete { get { return false; } }

    protected override bool CanPower
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

    protected override void ApplyPowerInternal(GameDevice device)
    {
        base.ApplyPowerInternal(device);
        win = true;
    }
}
