using UnityEngine;
using System.Collections;
using System;

public class LightGoalDevice : GameDevice {

    public override bool CanDelete { get { return false; } }

    protected override bool CanLight
    {
        get
        {
            return true;
        }
    }

    Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
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

    protected override void ApplyLightInternal(GameDevice device)
    {
        base.ApplyLightInternal(device);
        win = true;
    }
}
