using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ToolPanel : MonoBehaviour {

    public Text[] LevelText;
    public ToolButton ButtonPrefab;
    public Transform ButtonPanel;
    public float ButtonOffsetX;
    public float ButtonOffsetY;
    public ToolButtonConfig[] Config;
    public bool CollapseHidden;

    List<ToolButton> buttonPool = new List<ToolButton>();
    List<ToolButton> buttons = new List<ToolButton>();
    string lastLevelText;

    public static ToolPanel Current;

    void Awake()
    {
        Current = this;
    }

    void ClearButtons()
    {
        buttonPool.AddRange(buttons);
        for (int i = 0; i < buttonPool.Count; i++)
            buttonPool[i].gameObject.SetActive(false);
        buttons.Clear();
    }

    void OnDestroy()
    {
        ClearButtons();
    }

    bool firstUpdated = false;
    public void Configure()
    {
        ClearButtons();

        for (int i = 0; i < Config.Length; i++)
        {
            SpawnButton(Config[i].Tool, Config[i].Selector, Config[i].ToolName);
        }

        firstUpdated = false;
    }

    ToolButton SpawnButton(Sprite icon, Sprite selector, string toolName)
    {
        ToolButton btn;

        if (buttonPool.Count > 0)
        {
            btn = buttonPool[0];
            btn.gameObject.SetActive(true);
            buttonPool.RemoveAt(0);
        }
        else
        {
            btn = (ToolButton)Instantiate(ButtonPrefab);
            btn.transform.SetParent(ButtonPanel);
        }

        SetToolButtonPosition(btn, buttons.Count);

        btn.Icon = icon;
        btn.Selector = selector;
        btn.PartName = toolName;

        buttons.Add(btn);

        return btn;
    }

    void SetToolButtonPosition(ToolButton btn, int index)
    {
        RectTransform btnTrans = (RectTransform)btn.transform;
        Vector2 tmp = btnTrans.anchorMin;
        if (ButtonOffsetX > 0)
            tmp.x = index * ButtonOffsetX;
        if (ButtonOffsetY > 0)
            tmp.y = 1 - ((index + 1) * ButtonOffsetY);
        btnTrans.anchorMin = tmp;

        tmp = btnTrans.anchorMax;
        if (ButtonOffsetX > 0)
            tmp.x = (index + 1) * ButtonOffsetX;
        if (ButtonOffsetY > 0)
            tmp.y = 1 - (index * ButtonOffsetY);
        btnTrans.anchorMax = tmp;

        btnTrans.offsetMax = btnTrans.offsetMin = Vector2.zero;
        btnTrans.localScale = IDENTITY_V3;
    }

    static readonly Vector3 IDENTITY_V3 = new Vector3(1, 1, 1);

    public void Refresh()
    {
        // Apply level name
        if (LevelText != null)
        {
            if (GameState.LEVELS[GameState.LevelIndex].Name != lastLevelText)
            {
                lastLevelText = GameState.LEVELS[GameState.LevelIndex].Name;
                string levelTextFull = string.Format("Level {0}: {1}", GameState.LevelIndex + 1, lastLevelText);

                for (int i = 0; i < LevelText.Length; i++)
                    LevelText[i].text = levelTextFull;
            }
        }

        // Update buttons
        int hidden = 0;
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].RefreshIcon(!firstUpdated);
            if (CollapseHidden)
            {
                if (buttons[i].Visible)
                    SetToolButtonPosition(buttons[i], i - hidden);
                else
                    hidden++;
            }
        }

        firstUpdated = true;
    }
}

[System.Serializable]
public class ToolButtonConfig
{
    public Sprite Tool;
    public Sprite Selector;
    public string ToolName;
}
