using UnityEngine;
using System.Collections;

public class GearGoalDevice : GameDevice {

    public SpriteRenderer Indicator;

    public override bool CanDelete { get { return false; } }

    protected override bool CanCrank
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
        if (_r != null && Indicator != null)
            Indicator.sortingOrder = _r.sortingOrder + 1;
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

    protected override void ApplyCrankInternal(GameDevice device)
    {
        base.ApplyCrankInternal(device);
        win = true;
    }
}
