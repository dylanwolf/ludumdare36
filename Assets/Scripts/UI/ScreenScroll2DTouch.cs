using UnityEngine;
using System.Collections;

public class ScreenScroll2DTouch : MonoBehaviour {

    public float Speed = 0.1f;

    // Logic based on http://stackoverflow.com/questions/25323389/camera-2d-movement-android-unity
    void Update () {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * Speed * Time.deltaTime, -touchDeltaPosition.y * Speed * Time.deltaTime, 0);
        }

        MouseTouch.Touch? mouseTouch = MouseTouch.GetTouch(0);
        if (mouseTouch.HasValue && mouseTouch.Value.Phase == MouseTouch.TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = mouseTouch.Value.DeltaPosition;
            transform.Translate(-touchDeltaPosition.x * Speed * Time.deltaTime, -touchDeltaPosition.y * Speed * Time.deltaTime, 0);
        }


    }
}
