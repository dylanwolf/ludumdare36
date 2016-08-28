using UnityEngine;
using System.Collections;
using System;

public class DoorDevice : GameDevice {

    public override bool CanDelete
    {
        get
        {
            return false;
        }
    }

    protected override bool CanSwitch
    {
        get
        {
            return true;
        }
    }

    bool isOpen = false;

    public override bool BlocksLaser
    {
        get
        {
            return !isOpen;
        }
    }

    DeviceAnimation AnimationState;
    Animator _anim;


    void Awake()
    {
        _anim = GetComponent<Animator>();
    }


    public override void Initialize()
    {
        base.Initialize();
        isOpen = false;
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
        if (SwitchedBy.Count > 0)
        {
            if (AnimationState == DeviceAnimation.Stopped)
                AnimationState = DeviceAnimation.Starting;
            else if (AnimationState == DeviceAnimation.Ending)
                AnimationState = DeviceAnimation.Running;
        }
        else
        {
            isOpen = false;

            if (AnimationState == DeviceAnimation.Starting || AnimationState == DeviceAnimation.Running)
                AnimationState = DeviceAnimation.Ending;
        }

        ApplyAnimation();
    }

    const string ANIM_STARTING = "IsStarting";
    const string ANIM_RUNNING = "IsRunning";
    const string ANIM_ENDING = "IsEnding";
    void ApplyAnimation()
    {
        if (_anim != null)
        {
            _anim.SetBool(ANIM_STARTING, AnimationState == DeviceAnimation.Starting);
            _anim.SetBool(ANIM_RUNNING, AnimationState != DeviceAnimation.Stopped);
            _anim.SetBool(ANIM_ENDING, AnimationState == DeviceAnimation.Ending);
        }
    }

    protected override void ApplySwitchInternal(GameDevice device)
    {
        base.ApplySwitchInternal(device);
    }

    public void AnimationStartedComplete()
    {
        isOpen = true;
        AnimationState = DeviceAnimation.Running;
    }

    public void AnimationEndingComplete()
    {
        isOpen = false;
        AnimationState = DeviceAnimation.Stopped;
    }
}
