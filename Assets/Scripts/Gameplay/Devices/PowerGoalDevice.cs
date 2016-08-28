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

    Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

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
        ApplyAnimation();
    }

    const string ANIM_RUNNING = "IsRunning";
    void ApplyAnimation()
    {
        if (_anim != null)
        {
            _anim.SetBool(ANIM_RUNNING, win);
        }
    }

    protected override void ApplyPowerInternal(GameDevice device)
    {
        base.ApplyPowerInternal(device);
        win = true;
    }
}
