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
        if (IconRenderer != null)
        {
            if (Visible)
                IconRenderer.sprite = Icon;
            IconRenderer.enabled = Visible;
        }

        if (SelectorRenderer != null)
        {
            if (Visible && (PartName == GameState.EditorSelection))
                SelectorRenderer.sprite = Selector;
            SelectorRenderer.enabled = Visible && (PartName == GameState.EditorSelection);
        }

        if (CountText != null && (Count != lastCount || force))
        {
            countText = Count.HasValue ? Count.Value.ToString() : string.Empty;
            for (int i = 0; i < CountText.Length; i++)
            {
                if (Count.HasValue)
                {
                    CountText[i].text = countText;
                }
                else
                {
                    CountText[i].enabled = false;
                }
            }
            lastCount = Count;
        }
    }

    public void ButtonClick()
    {
        return;
    }
}
