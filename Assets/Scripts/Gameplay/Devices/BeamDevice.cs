using UnityEngine;
using System.Collections;
using System;

public class BeamDevice : GameDevice {

    public Transform BeamPivot;
    public Transform BeamScale;
    public int[] Direction = new int[2];

    public SpriteRenderer StartSprite;
    public SpriteRenderer EndSprite;
    public SpriteRenderer BeamSprite;

    public Transform EndpointPivots;
    public Transform LaserEnd;

    Transform scaleTransform;
    Transform spriteTransform;
    Transform pivotTransform;

    void Awake()
    {
        pivotTransform = BeamPivot.transform;
        scaleTransform = BeamScale.transform;
        spriteTransform = BeamSprite.transform;
    }

    protected override void Start()
    {
        base.Start();
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        BlockLength = 0;
        SetScale(0);
        if (_r != null) _r.enabled = true;
        if (StartSprite != null) StartSprite.enabled = true;
        if (BeamSprite != null) BeamSprite.enabled = true;
        if (EndSprite != null) EndSprite.enabled = true;
    }

    public override void Disable()
    {
        base.Disable();
        if (_r != null) _r.enabled = false;
        if (StartSprite != null) StartSprite.enabled = false;
        if (BeamSprite != null) BeamSprite.enabled = false;
        if (EndSprite != null) EndSprite.enabled = false;
    }

    public void SetTile(int x, int y)
    {
        TileX = x;
        TileY = y;
        targetTileX = x;
        targetTileY = y;
    }

    void SetLayer(SpriteRenderer r, int layer)
    {
        if (r != null)
            r.sortingOrder = layer;
    }

    public void SetDirectionAndLayer(int x, int y)
    {
        Direction[0] = x;
        Direction[1] = y;

        SetLayer(_r, (TileY * 10) + 1);
        SetLayer(StartSprite, (TileY * 10) + 2);
        SetLayer(BeamSprite, (TileY * 10) + 1);
        SetLayer(EndSprite, (TileY * 10) + 2);

        if (y == 0)
        {
            if (x == 1)
                SetPivotRotation(0);
            else if (x == -1)
                SetPivotRotation(180);
        }
        else if (y == 1)
        {
            SetPivotRotation(270);
        }
        else if (y == -1)
        {
            SetPivotRotation(90);
        }
    }

    const float SPRITE_SIZE = 0.64f;

    Vector3 tmpPos;
    public void SetScale(float blocksLength)
    {
        SetScaleX(
            (blocksLength *
                (((Direction[0] != 0) ? GameEngine.TileSize.x : GameEngine.TileSize.y) / SPRITE_SIZE))
                //- scaleTransform.localPosition.x
        );

        if (pivotTransform != null && LaserEnd != null && scaleTransform != null && spriteTransform != null)
        {
            tmpPos = LaserEnd.localPosition;
            tmpPos.x = pivotTransform.localPosition.x + scaleTransform.localPosition.x + (spriteTransform.localPosition.x * scaleTransform.localScale.x * 2);
            LaserEnd.localPosition = tmpPos;
        }
    }

    void SetPivotRotation(float angle)
    {
        if (EndpointPivots != null && pivotTransform != null)
            EndpointPivots.localRotation = pivotTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    Vector3 tmpScale;
    void SetScaleX(float scaleX)
    {
        if (scaleTransform != null)
        {
            tmpScale = scaleTransform.localScale;
            tmpScale.x = scaleX;
            scaleTransform.localScale = tmpScale;
        }
    }

    void SetScaleY(float scaleY)
    {
        tmpScale = scaleTransform.localScale;
        tmpScale.y = scaleY;
        scaleTransform.localScale = tmpScale;
    }

    protected override void ResetTickStateInternal()
    {
        
    }

    protected override void CleanupAfterTickInternal()
    {
        
    }


    public float Speed = 1.0f;
    public float StartSpeed = 1.0f;
    public float MinScaleY = 0.25f;
    public float PulseScaleY = 0.1f;
    public float InitialWaitLength = 0.5f;
    public float InitialScaleUpLength = 1.0f;
    public float PulseLength = 1.0f;
    public float BlockLength = 0;
    float scaleUpLength;

    bool pulse;
    int targetTileX;
    int nextTileX;
    int targetTileY;
    int nextTileY;

    float pulseTimer = 0;
    int modifiedBlockLength = 0;

    int testX;
    int testY;

    protected override void TickInternal()
    {
        pulse = true;

        modifiedBlockLength = Mathf.FloorToInt(Mathf.Clamp(BlockLength - scaleTransform.localPosition.x, 0, float.MaxValue));

        // Determine what the current target tile is 
        targetTileX = TileX + (modifiedBlockLength * Direction[0]);
        targetTileY = TileY + (modifiedBlockLength * Direction[1]);

        // Test interposing blocks
        for (int i = 0; i < modifiedBlockLength; i++)
        {
            testX = TileX + (Direction[0] * (i + 1));
            testY = TileY + (Direction[1] * (i + 1));

            if (GameState.Devices[testY, testX] != null && GameState.Devices[testY, testX].BlocksLaser)
            {
                targetTileX = testX;
                targetTileY = testY;
                BlockLength = i;
            }

        }

        nextTileX = targetTileX + Direction[0];
        nextTileY = targetTileY + Direction[1];

        GameEngine.Current.Light(this, targetTileX, targetTileY);
        GameEngine.Current.Light(this, nextTileX, nextTileY);

        // Grow laser if unblocked
        if (nextTileX >= 0 && nextTileY >= 0 && nextTileX < GameState.LevelWidth && nextTileY < GameState.LevelHeight &&
            (GameState.Devices[nextTileY, nextTileX] == null || !GameState.Devices[nextTileY, nextTileX].BlocksLaser))
        {
            if (BlockLength <= InitialWaitLength)
            {
                BlockLength += Time.deltaTime * StartSpeed;
                SetScaleY(1 - MinScaleY);
            }
            else if (BlockLength <= InitialScaleUpLength)
            {
                BlockLength += Time.deltaTime * StartSpeed;
                SetScaleY(1 - (MinScaleY * Mathf.Sin(
                        (1 - ((BlockLength - InitialWaitLength) / (InitialScaleUpLength - InitialWaitLength))) * Mathf.PI / 2
                    )));
                pulse = false;
            }
            else
            {
                BlockLength += Time.deltaTime * Speed;
            }
        }


        // Pulse laser if it's in its final state
        if (pulse)
        {
            pulseTimer += Time.deltaTime;
            if (pulseTimer > PulseLength)
                pulseTimer = pulseTimer % PulseLength;

            SetScaleY(
                1.5f +
                    (
                        PulseScaleY * 0.5f * Mathf.Cos(2 * ((pulseTimer + 0.5f) / PulseLength) * Mathf.PI)
                    )
                );
        }

        SetScale(BlockLength);
    }
}
