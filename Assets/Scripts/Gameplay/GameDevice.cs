using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GameDevice : MonoBehaviour, IObjectPoolable {

    public int TileX;
    public int TileY;

    [System.NonSerialized]
    public bool HasPoweredThisTick;
    [System.NonSerialized]
    public bool HasCrankedThisTick;
    [System.NonSerialized]
    public bool HasLitThisTick;

    public bool CanDelete = true;

    protected List<GameDevice> PoweredBy = new List<GameDevice>();
    protected List<GameDevice> CrankedBy = new List<GameDevice>();
    protected List<GameDevice> LitBy = new List<GameDevice>();

    protected SpriteRenderer _r;

    void Start()
    {
        _r = GetComponent<SpriteRenderer>();
    }

    public virtual void Initialize()
    {
        if (_r != null)
        {
            _r.enabled = true;
        }
    }

    void Update()
    {
        if (_r != null)
            _r.sortingOrder = TileY;
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
        HasCrankedThisTick = false;
        HasPoweredThisTick = false;
        HasLitThisTick = false;

        PoweredBy.Clear();
        CrankedBy.Clear();
        LitBy.Clear();
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
        throw new System.NotImplementedException();
    }

    protected virtual void ApplyPowerInternal(GameDevice device)
    {
    }

    protected virtual void ApplyCrankInternal(GameDevice device)
    {

    }

    protected virtual void ApplyLightInternal(GameDevice device)
    {
        throw new System.NotImplementedException();
    }

    protected virtual bool CanPower { get { return false; } }
    protected virtual bool CanCrank { get { return false; } }
    protected virtual bool CanLight { get { return false; } }

    protected abstract void ResetTickStateInternal();
    protected abstract void TickInternal();
    protected abstract void CleanupAfterTickInternal();
}
