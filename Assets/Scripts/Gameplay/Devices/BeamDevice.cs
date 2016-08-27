using UnityEngine;
using System.Collections;
using System;

public class BeamDevice : GameDevice {

    public Transform BeamPivot;
    public Transform BeamScale;
    public bool ResetAnimation = false;
    public int[] Direction = new int[2];

    public SpriteRenderer StartSprite;
    public SpriteRenderer EndSprite;
    public SpriteRenderer BeamSprite;

    public Transform EndpointPivots;
    public Transform LaserEnd;

    Transform scaleTransform;
    Transform spriteTransform;
    Transform pivotTransform;

    void Start()
    {
        pivotTransform = BeamPivot.transform;
        scaleTransform = BeamScale.transform;
        spriteTransform = BeamSprite.transform;
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        SetDirection(0, -1);
        BlockLength = 0;
    }

    public void SetDirection(int x, int y)
    {
        Direction[0] = x;
        Direction[1] = y;

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

    Vector3 tmpPos;
    public void SetScale(float blocksLength)
    {
        SetScaleX((blocksLength - BeamPivot.localPosition.x) * GameEngine.TileSize.x);

        tmpPos = LaserEnd.localPosition;
        tmpPos.x = pivotTransform.localPosition.x + scaleTransform.localPosition.x + (spriteTransform.localPosition.x * scaleTransform.localScale.x * 2);
        LaserEnd.localPosition = tmpPos;
    }

    void SetPivotRotation(float angle)
    {
        EndpointPivots.localRotation = pivotTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    Vector3 tmpScale;
    void SetScaleX(float scaleX)
    {
        tmpScale = scaleTransform.localScale;
        tmpScale.x = scaleX;
        scaleTransform.localScale = tmpScale;
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

    protected override void TickInternal()
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
    float BlockLength = 0;
    float scaleUpLength;
    void Update()
    {
        if (ResetAnimation)
        {
            BlockLength = 0;
            ResetAnimation = false;
        }

        if (BlockLength <= InitialWaitLength)
        {
            BlockLength += Time.deltaTime * StartSpeed;
            SetScaleY(1 - MinScaleY);
        }
        else if (BlockLength <= InitialScaleUpLength)
        {
            BlockLength += Time.deltaTime * StartSpeed;
            SetScaleY(1 - (MinScaleY * Mathf.Sin(
                    (1-((BlockLength - InitialWaitLength) / (InitialScaleUpLength - InitialWaitLength))) * Mathf.PI / 2
                )));
        }
        else
        {
            BlockLength += Time.deltaTime * Speed;

            SetScaleY(
                1.5f +
                    (
                        PulseScaleY * 0.5f * Mathf.Cos(2 * ((BlockLength + 0.5f)/PulseLength) * Mathf.PI)
                    )
                );

        }
        SetScale(BlockLength);
    }
}
