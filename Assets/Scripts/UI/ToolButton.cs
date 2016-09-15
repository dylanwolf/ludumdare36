using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToolButton : MonoBehaviour {

    public Sprite Icon;
    public Sprite Selector;
    public int? Count;
    public string PartName;
    public bool Visible;

    public Image IconRenderer;
    public Image SelectorRenderer;
    public Graphic[] CountFrame;
    public Text[] CountText;

    int? lastCount = null;
    string countText = string.Empty;

    public void RefreshIcon(bool force)
    {
        // Get the total count of items, if this is a limited part, and the visibility of the item
        if (PartName != null && GameState.Parts != null)
        {
            if (GameState.Parts.ContainsKey(PartName))
            {
                Count = GameState.Parts[PartName];
                Visible = true;
            }
            else if (PartName == GameState.EDITOR_DELETE)
            {
                Count = null;
                Visible = true;
            }
            else
            {
                Count = null;
                Visible = false;
            }
        }
        else
        {
            Count = null;
            Visible = false;
        }

        // Apply to the controls
        if (Visible)
        {
            gameObject.SetActive(true);

            if (IconRenderer != null)
                IconRenderer.sprite = Icon;

            if (SelectorRenderer != null)
            {
                if (PartName == GameState.EditorSelection)
                {
                    SelectorRenderer.sprite = Selector;
                    SelectorRenderer.enabled = true;
                }
                else
                {
                    SelectorRenderer.enabled = false;
                }
            }

            if (CountText != null && (Count != lastCount || force))
            {
                countText = Count.HasValue ? Count.Value.ToString() : string.Empty;
                for (int i = 0; i < CountText.Length; i++)
                {
                    if (Count.HasValue)
                    {
                        CountText[i].text = countText;
                        CountText[i].enabled = true;
                    }
                    else
                    {
                        CountText[i].enabled = false;
                    }
                }

                if (CountFrame != null)
                {
                    for (int i = 0; i < CountFrame.Length; i++)
                    {
                        CountFrame[i].enabled = Count.HasValue;
                    }
                }
                lastCount = Count;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void SelectTool(string toolName, Graphic icon)
    {
        if (GameState.EditorSelection != toolName)
        {
            GameEngine.Current.SelectTool(toolName);
            SelectorRenderer.enabled = true;
            SelectorRenderer.transform.position = icon.transform.position;
        }
        else
        {
            GameEngine.Current.SelectTool(null);
            SelectorRenderer.enabled = false;
        }
    }

    public void ButtonClick()
    {
        SelectTool(PartName, IconRenderer);
    }
}
