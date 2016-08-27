using UnityEngine;
using System.Collections;

public class ToolSelector : MonoBehaviour {

	public void DeselectTool()
    {
        GameEngine.Current.SelectTool(null);
    }

    public void DeleteTool()
    {
        GameEngine.Current.SelectTool(GameState.EDITOR_DELETE);
    }

    public void GearTool()
    {
        GameEngine.Current.SelectTool(GameEngine.GEAR_POOL);
    }

    public void GeneratorTool()
    {
        GameEngine.Current.SelectTool(GameEngine.GENERATOR_POOL);
    }

    public void RepeaterTool()
    {
        GameEngine.Current.SelectTool(GameEngine.REPEATER_POOL);
    }

    public void LaserTool()
    {
        GameEngine.Current.SelectTool(GameEngine.LASER_POOL);
    }

    public void MirrorTool()
    {
        GameEngine.Current.SelectTool(GameEngine.MIRROR_POOL);
    }
}
