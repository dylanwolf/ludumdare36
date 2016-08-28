using UnityEngine;
using System.Collections;

public class RepeaterDevice : GameDevice {

    protected override bool CanPower
    {
        get { return true; }
    }

    Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    bool applyPower = false;

    public override void Initialize()
    {
        base.Initialize();
        applyPower = false;
    }

    protected override void ResetTickStateInternal()
    {
        applyPower = false;
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
            _anim.SetBool(ANIM_RUNNING, applyPower);
        }
    }

    protected override void ApplyPowerInternal(GameDevice device)
    {
        base.ApplyPowerInternal(device);
        GameEngine.Current.Power(this, TileX - 1, TileY);
        GameEngine.Current.Power(this, TileX + 1, TileY);
        GameEngine.Current.Power(this, TileX, TileY - 1);
        GameEngine.Current.Power(this, TileX, TileY + 1);
        applyPower = true;
    }
}
