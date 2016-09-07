using UnityEngine;
using System.Collections;

public class ScreenScroll2DTouch : MonoBehaviour {

    public float Speed = 0.1f;
    public BoxCollider2D BoundingBox;

    bool updatedPosition = false;

    // Logic based on http://stackoverflow.com/questions/25323389/camera-2d-movement-android-unity
    void Update () {

        updatedPosition = false;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * Speed * Time.deltaTime, -touchDeltaPosition.y * Speed * Time.deltaTime, 0);
            updatedPosition = true;
        }

        MouseTouch.Touch? mouseTouch = MouseTouch.GetTouch(0);
        if (mouseTouch.HasValue && mouseTouch.Value.Phase == MouseTouch.TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = mouseTouch.Value.DeltaPosition;
            transform.Translate(-touchDeltaPosition.x * Speed * Time.deltaTime, -touchDeltaPosition.y * Speed * Time.deltaTime, 0);
            updatedPosition = true;
        }

        if (updatedPosition && BoundingBox != null)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, BoundingBox.transform.position.x - (BoundingBox.size.x/2.0f), BoundingBox.transform.position.x + (BoundingBox.size.x / 2.0f));
            pos.y = Mathf.Clamp(pos.y, BoundingBox.transform.position.y - (BoundingBox.size.y / 2.0f), BoundingBox.transform.position.y + (BoundingBox.size.y / 2.0f));
            transform.position = pos;
        }
    }
}
