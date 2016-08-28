using UnityEngine;
using System.Collections;
using System;

public class GearDevice : GameDevice {

    protected override bool CanCrank
    {
        get { return true; }
    }

    DeviceAnimation AnimationState;
    Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public bool applyCrank = false;

    public override void Initialize()
    {
        base.Initialize();
        applyCrank = false;
        AnimationState = DeviceAnimation.Stopped;
    }

    protected override void ResetTickStateInternal()
    {
    }

    protected override void TickInternal()
    {
    }

    protected override void CleanupAfterTickInternal()
    {
        if (CrankedBy.Count > 0)
        {
            if (AnimationState == DeviceAnimation.Stopped)
                AnimationState = DeviceAnimation.Starting;
            else if (AnimationState == DeviceAnimation.Ending)
                AnimationState = DeviceAnimation.Running;
        }
        else
        {
            applyCrank = false;

            if (AnimationState == DeviceAnimation.Starting || AnimationState == DeviceAnimation.Running)
                AnimationState = DeviceAnimation.Ending;
        }

        ApplyAnimation();
    }

    const string ANIM_RUNNING = "IsRunning";
    void ApplyAnimation()
    {
        if (_anim != null)
        {
            _anim.SetBool(ANIM_RUNNING, AnimationState != DeviceAnimation.Stopped);
        }
    }

    protected override void ApplyCrankInternal(GameDevice device)
    {
        base.ApplyCrankInternal(device);
        GameEngine.Current.Crank(this, TileX - 1, TileY);
        GameEngine.Current.Crank(this, TileX + 1, TileY);
        GameEngine.Current.Crank(this, TileX, TileY - 1);
        GameEngine.Current.Crank(this, TileX, TileY + 1);
        applyCrank = true;
    }
}
