using UnityEngine;
using System.Collections;

public class ScreenScroll2DTouch : MonoBehaviour {

    public float Speed = 0.1f;
    public BoxCollider2D BoundingBox;

    public static ScreenScroll2DTouch Current;

    void Awake()
    {
        Current = this;
    }

    public void CancelMove()
    {
        isTouchCanceled = true;
        isMouseCanceled = true;
    }

    bool isTouchCanceled = false;
    bool isMouseCanceled = false;
    bool updatedPosition = false;

    // Logic based on http://stackoverflow.com/questions/25323389/camera-2d-movement-android-unity
    void Update () {

        updatedPosition = false;

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved && !isTouchCanceled)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                transform.Translate(-touchDeltaPosition.x * Speed * Time.deltaTime, -touchDeltaPosition.y * Speed * Time.deltaTime, 0);
                updatedPosition = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                isTouchCanceled = false;
            }
        }
        else
        {
            isTouchCanceled = false;
        }

        MouseTouch.Touch? mouseTouch = MouseTouch.GetTouch(0);
        if (mouseTouch.HasValue && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (mouseTouch.Value.Phase == MouseTouch.TouchPhase.Moved && !isMouseCanceled)
            {
                Vector2 touchDeltaPosition = mouseTouch.Value.DeltaPosition;
                transform.Translate(-touchDeltaPosition.x * Speed * Time.deltaTime, -touchDeltaPosition.y * Speed * Time.deltaTime, 0);
                updatedPosition = true;
            }
            else if (mouseTouch.Value.Phase == MouseTouch.TouchPhase.Canceled || mouseTouch.Value.Phase == MouseTouch.TouchPhase.Ended)
            {
                isMouseCanceled = false;
            }
        }
        else
        {
            isMouseCanceled = false;
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
