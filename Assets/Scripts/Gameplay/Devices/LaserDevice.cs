using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserDevice : GameDevice {

    public BeamDevice[] beams = new BeamDevice[4];
    public bool[] beamsOn = new bool[4];

    public Transform BeamAttach;

    public override int StackLayerMultiplier
    {
        get
        {
            return 10;
        }
    }

    protected override bool CanPower
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

    static readonly int[] beamLayerOffsets = new int[] { 20, 20, 0, 30 };

    protected override void ApplyPowerInternal(GameDevice device)
    {
        base.ApplyPowerInternal(device);
        HasPoweredThisTick = false;

        // beam 0 = 1,0
        if (device.TileX == TileX - 1 && device.TileY == TileY)
            beamsOn[0] = true;
        // beam 1 = -1,0
        else if (device.TileX == TileX + 1 && device.TileY == TileY)
            beamsOn[1] = true;
        // beam 2 = 0, -1
        else if (device.TileX == TileX && device.TileY == TileY + 1)
            beamsOn[2] = true;
        // beam 3 = 0, 1
        else if (device.TileX == TileX && device.TileY == TileY - 1)
            beamsOn[3] = true;
    }
}
