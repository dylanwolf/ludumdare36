using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GameDevice : MonoBehaviour, IObjectPoolable {

    public virtual bool HasSetWinCondition { get { return false; } }
    public virtual bool IsWinCondition { get { return false; } }
    public virtual bool BlocksLaser {  get { return true; } }

    public SpriteRenderer[] ImageStack;

    public int TileX;
    public int TileY;

    public string PartName;

    [System.NonSerialized]
    public bool HasPoweredThisTick;
    [System.NonSerialized]
    public bool HasCrankedThisTick;
    [System.NonSerialized]
    public bool HasLitThisTick;
    [System.NonSerialized]
    public bool HasSwitchedThisTick;

    public const int TileLayerMultiplier = 100;

    public virtual bool CanDelete { get { return true; } }

    protected List<GameDevice> PoweredBy = new List<GameDevice>();
    protected List<GameDevice> CrankedBy = new List<GameDevice>();
    protected List<GameDevice> LitBy = new List<GameDevice>();
    protected List<GameDevice> SwitchedBy = new List<GameDevice>();

    protected SpriteRenderer _r;

    protected virtual void Start()
    {
        _r = GetComponent<SpriteRenderer>();
    }

    public virtual void Initialize()
    {
        if (_r != null)
        {
            _r.enabled = true;
        }

        OrderSprites();
        SwitchX = null;
        SwitchY = null;
    }

    protected virtual void Update()
    {
        if (_r != null)
            _r.sortingOrder = TileY * TileLayerMultiplier;
    }

    public virtual void Disable()
    {
        if (_r != null)
            _r.enabled = false;
    }

    private int cleanupIndex;
    public void CleanupReferences()
    {
        CleanupReferencesList(PoweredBy);
        CleanupReferencesList(CrankedBy);
        CleanupReferencesList(LitBy);
        CleanupReferencesList(SwitchedBy);
    }

    void CleanupReferencesList(List<GameDevice> lst)
    {
        cleanupIndex = 0;
        while (lst.Count > 0 && cleanupIndex < lst.Count)
        {
            if (lst[cleanupIndex].isActiveAndEnabled)
                cleanupIndex++;
            else
                lst.RemoveAt(cleanupIndex);
        }
    }

    public void ResetTickState()
    {
        OrderSprites();

        HasCrankedThisTick = false;
        HasPoweredThisTick = false;
        HasLitThisTick = false;
        HasSwitchedThisTick = false;

        PoweredBy.Clear();
        CrankedBy.Clear();
        LitBy.Clear();
        SwitchedBy.Clear();

        ResetTickStateInternal();
    }

    void OrderSprites()
    {
        if (_r != null && ImageStack != null)
        {
            for (int i = 0; i < ImageStack.Length; i++)
            {
                ImageStack[i].sortingOrder = _r.sortingOrder + i + 1;
            }
        }
    }

    public void Tick()
    {
        CleanupReferences();
        TickInternal();
    }

    public void CleanupAfterTick()
    {
        CleanupAfterTickInternal();
    }

    public void ApplyPower(GameDevice device) {
        if (CanPower)
        {
            PoweredBy.Add(device);

            if (!HasPoweredThisTick)
            {
                HasPoweredThisTick = true;
                ApplyPowerInternal(device);
            }
        }
    }

    public void ApplyCrank(GameDevice device)
    {
        if (CanCrank)
        {
            CrankedBy.Add(device);

            if (!HasCrankedThisTick)
            {
                HasCrankedThisTick = true;
                ApplyCrankInternal(device);
            }
        }
    }

    public void ApplyLight(GameDevice device) {
        if (CanLight)
        {
            LitBy.Add(device);

            if (!HasLitThisTick)
            {
                HasLitThisTick = true;
                ApplyLightInternal(device);
            }
        }
    }

    protected virtual void ApplyPowerInternal(GameDevice device)
    {
    }

    protected virtual void ApplyCrankInternal(GameDevice device)
    {

    }

    protected virtual void ApplyLightInternal(GameDevice device)
    {
        
    }

    protected virtual void ApplySwitchInternal(GameDevice device)
    {

    }

    protected virtual bool CanPower { get { return false; } }
    protected virtual bool CanCrank { get { return false; } }
    protected virtual bool CanLight { get { return false; } }
    protected virtual bool CanSwitch { get { return false; } }

    protected abstract void ResetTickStateInternal();
    protected abstract void TickInternal();
    protected abstract void CleanupAfterTickInternal();

    protected int? SwitchX;
    protected int? SwitchY;

    public void SetSwitchTarget(int x, int y)
    {
        SwitchX = x;
        SwitchY = y;
    }

    public void ApplySwitch(GameDevice device)
    {
        if (CanSwitch)
        {
            SwitchedBy.Add(device);

            if (!HasSwitchedThisTick)
            {
                HasSwitchedThisTick = true;
                ApplySwitchInternal(device);
            }
        }
    }

    protected void DoSwitch()
    {
        if (SwitchX.HasValue && SwitchY.HasValue)
            GameEngine.Current.Switch(this, SwitchX.Value, SwitchY.Value);
    }
}
