using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public int TileX;
    public int TileY;

    void OnMouseUpAsButton()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            GameEngine.Current.ApplyTool(TileX, TileY);
    }
}
