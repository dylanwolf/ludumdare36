using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FinishedLevelWindow : GenericWindow {

    public static FinishedLevelWindow Current;

    public Graphic[] NextLevelControls;

    void Awake()
    {
        Current = this;
    }

    public void ShowNextLevel(bool show)
    {
        for (int i = 0; i < NextLevelControls.Length; i++)
        {
            NextLevelControls[i].enabled = show;
        }
    }

    public void StartNextLevel()
    {
        this.Hide();
        GameEngine.Current.LoadCurrentLevel();
    }
}
