using UnityEngine;
using System.Collections;

public class GeneratorDevice : GameDevice {

    DeviceAnimation AnimationState;
    Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    protected override bool CanCrank
    {
        get { return true; }
    }

    bool applyPower = false;

    public override void Initialize()
    {
        base.Initialize();
        applyPower = false;
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
            applyPower = false;

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

    public void AnimationStartedComplete()
    {
        applyPower = true;
        AnimationState = DeviceAnimation.Running;
    }

    public void AnimationEndingComplete()
    {
        applyPower = false;
        AnimationState = DeviceAnimation.Stopped;
    }

    protected override void ApplyCrankInternal(GameDevice device)
    {
        base.ApplyCrankInternal(device);

        if (applyPower)
        {
            GameEngine.Current.Power(this, TileX - 1, TileY);
            GameEngine.Current.Power(this, TileX + 1, TileY);
            GameEngine.Current.Power(this, TileX, TileY - 1);
            GameEngine.Current.Power(this, TileX, TileY + 1);
        }
    }
}
