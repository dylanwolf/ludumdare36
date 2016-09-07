using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public int TileX;
    public int TileY;

    void OnMouseDown()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (GameEngine.Current.ApplyTool(TileX, TileY))
                ScreenScroll2DTouch.Current.CancelMove();
        }
    }

    void OnMouseOver()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(1))
        {
            GameEngine.Current.ApplyTool(GameState.EDITOR_DELETE, TileX, TileY);
        }
    }
}
