using UnityEngine;
using System.Collections;

public class MirrorDevice : GameDevice {

    public BeamDevice[] beams = new BeamDevice[4];
    public bool[] beamsOn = new bool[4];

    public Transform BeamAttach;

    public Sprite ActiveSprite;
    public Sprite InactiveSprite;

    protected override bool CanLight
    {
        get { return true; }
    }

    public override void Initialize()
    {
        base.Initialize();
        for (int i = 0; i < beams.Length; i++)
        {
            beamsOn[i] = false;
            if (beams[i] != null)
            {
                ObjectPools.Despawn(beams[i]);
                beams[i] = null;
            }
        }
    }

    public override void Disable()
    {
        base.Disable();
        for (int i = 0; i < beams.Length; i++)
        {
            beamsOn[i] = false;
            if (beams[i] != null)
            {
                ObjectPools.Despawn(beams[i]);
                beams[i] = null;
            }
        }
    }

    protected override void ResetTickStateInternal()
    {
        for (int i = 0; i < beamsOn.Length; i++)
        {
            beamsOn[i] = false;
            if (beams[i] != null)
                beams[i].ResetTickState();
        }
    }

    protected override void TickInternal()
    {
        for (int i = 0; i < beamsOn.Length; i++)
        {
            if (beams[i] != null)
                beams[i].Tick();
        }
    }

    protected override void CleanupAfterTickInternal()
    {
        if (_r != null)
            _r.sprite = (PoweredBy.Count > 0) ? ActiveSprite : InactiveSprite;

        for (int i = 0; i < beams.Length; i++)
        {
            UpdateBeam(i);
            if (beams[i] != null)
                beams[i].CleanupAfterTick();
        }
    }

    const string BEAM_POOL = "Beams";
    void UpdateBeam(int index)
    {
        if (beamsOn[index])
        {
            if (beams[index] == null)
            {
                beams[index] = ObjectPools.Spawn<BeamDevice>(BEAM_POOL, BeamAttach.position, BeamAttach);
                beams[index].SetTile(TileX, TileY);
                beams[index].SetDirectionAndLayer(
                        (index == 0) ? 1 : ((index == 1) ? -1 : 0),
                        (index == 2) ? -1 : ((index == 3) ? 1 : 0),
                        beamLayerOffsets[index]
                    );
            }
            else
            {
                beams[index].Tick();
            }
        }
        else if (beams[index] != null)
        {
            ObjectPools.Despawn(beams[index]);
            beams[index] = null;
        }
    }

    static readonly int[] beamLayerOffsets = new int[] { 30, 10, 20, 20 };

    protected override void ApplyLightInternal(GameDevice device)
    {
        base.ApplyLightInternal(device);
        HasLitThisTick = false;

        // beam 0 = 1,0
        // beam 1 = -1,0
        // beam 2 = 0, -1
        // beam 3 = 0, 1

        if (device.TileX != TileX && device.TileY == TileY)
        {
            beamsOn[2] = true;
            beamsOn[3] = true;
        }
        else if (device.TileY != TileY && device.TileX == TileX)
        {
            beamsOn[0] = true;
            beamsOn[1] = true;
        }
    }
}
