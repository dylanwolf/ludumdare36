using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ToolSelector : MonoBehaviour {

    public Graphic DeleteIcon;

    public Graphic GearIcon;
    public Text GearCount;

    public Graphic GeneratorIcon;
    public Text GeneratorCount;

    public Graphic RepeaterIcon;
    public Text RepeaterCount;

    public Graphic LaserIcon;
    public Text LaserCount;

    public Graphic MirrorIcon;
    public Text MirrorCount;

    public Text LevelText;

    public Graphic Selector;

    public static ToolSelector Current;

    void Awake()
    {
        Current = this;
    }

    public void SetSlots()
    {
        cache.Clear();
        Selector.enabled = false;
        GameEngine.Current.SelectTool(null);
        GearCount.enabled = GearIcon.enabled = GameState.Parts.ContainsKey(GameEngine.GEAR_POOL);
        GeneratorCount.enabled = GeneratorIcon.enabled = GameState.Parts.ContainsKey(GameEngine.GENERATOR_POOL);
        RepeaterCount.enabled = RepeaterIcon.enabled = GameState.Parts.ContainsKey(GameEngine.REPEATER_POOL);
        LaserCount.enabled = LaserIcon.enabled = GameState.Parts.ContainsKey(GameEngine.LASER_POOL);
        MirrorCount.enabled = MirrorIcon.enabled = GameState.Parts.ContainsKey(GameEngine.MIRROR_POOL);
        UpdateCounts();
    }

    public void SetLevelName(int levelIndex, string name)
    {
        LevelText.text = string.Format("Level {0}: {1}", levelIndex + 1, name);
    }

    Dictionary<string, int> cache = new Dictionary<string, int>();

    void UpdateCount(Text text, string poolName)
    {
        if (GameState.Parts.ContainsKey(poolName))
        {
            int count = GameState.Parts[poolName];
            if (cache.ContainsKey(poolName) && cache[poolName] == count)
                return;
            text.text = GameState.Parts[poolName].ToString();
            cache[poolName] = count;
        }
    }

    public void UpdateCounts()
    {
        UpdateCount(GearCount, GameEngine.GEAR_POOL);
        UpdateCount(GeneratorCount, GameEngine.GENERATOR_POOL);
        UpdateCount(RepeaterCount, GameEngine.REPEATER_POOL);
        UpdateCount(LaserCount, GameEngine.LASER_POOL);
        UpdateCount(MirrorCount, GameEngine.MIRROR_POOL);
    }

    public void DeleteTool()
    {
        if (GameState.EditorSelection != GameState.EDITOR_DELETE)
        {
            SoundBoard.Play(SOUND_SELECT);
            GameEngine.Current.SelectTool(GameState.EDITOR_DELETE);
            Selector.enabled = true;
            Selector.transform.position = DeleteIcon.transform.position;
        }
        else
        {
            GameEngine.Current.SelectTool(null);
            Selector.enabled = false;
        }
    }

    const string SOUND_SELECT = "ChangeSelection";

    void SelectTool(string toolName, Graphic icon)
    {
        if (GameState.EditorSelection != toolName)
        {
            SoundBoard.Play(SOUND_SELECT);
            GameEngine.Current.SelectTool(toolName);
            Selector.enabled = true;
            Selector.transform.position = icon.transform.position;
        }
        else
        {
            GameEngine.Current.SelectTool(null);
            Selector.enabled = false;
        }
    }

    public void GearTool()
    {
        SelectTool(GameEngine.GEAR_POOL, GearIcon);
    }

    public void GeneratorTool()
    {
        SelectTool(GameEngine.GENERATOR_POOL, GeneratorIcon);
    }

    public void RepeaterTool()
    {
        SelectTool(GameEngine.REPEATER_POOL, RepeaterIcon);
    }

    public void LaserTool()
    {
        SelectTool(GameEngine.LASER_POOL, LaserIcon);
    }

    public void MirrorTool()
    {
        SelectTool(GameEngine.MIRROR_POOL, MirrorIcon);
    }
}
