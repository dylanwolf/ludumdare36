using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseTouch : MonoBehaviour {

    public struct Touch
    {
        public TouchPhase Phase;
        public Vector2 Position;
        public Vector2 DeltaPosition;
    }

    public enum TouchPhase
    {
        Stationary,
        Canceled,
        Moved,
        Began,
        Ended
    }

    static Vector2? lastMousePosition;
    public int MouseButtonCount = 2;
    static Touch?[] Touches = null;
    static bool[] processedThisFrame = null;

    void Awake()
    {
        Touches = new Touch?[MouseButtonCount];
        processedThisFrame = new bool[MouseButtonCount];
    }

    // Logic based on processedFrame
    public static Touch? GetTouch(int button)
    {
        if (Touches == null || button < 0 || button >= Touches.Length)
            return null;

        if (!processedThisFrame[button])
        {
            if (Input.GetMouseButtonUp(button) || Input.GetMouseButton(button))
            {
                Touch t = new Touch();

                t.Phase = TouchPhase.Moved;

                if (Input.GetMouseButtonDown(0) && Input.GetMouseButtonUp(0))
                    t.Phase = TouchPhase.Canceled;
                else if (Input.GetMouseButtonUp(0))
                    t.Phase = TouchPhase.Ended;
                else if (Input.GetMouseButtonDown(0))
                    t.Phase = TouchPhase.Began;

                t.Position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 last = lastMousePosition.HasValue ? new Vector2(lastMousePosition.Value.x, lastMousePosition.Value.y) : t.Position;
                t.DeltaPosition = t.Phase == TouchPhase.Began ? Vector2.zero : (t.Position - last);

                Touches[button] = t;
            }

            processedThisFrame[button] = true;
        }

        return Touches[button];
    }

    void LateUpdate()
    {
        lastMousePosition = Input.mousePosition;

        if (Touches != null)
            for (int i = 0; i < Touches.Length; i++)
            {
                Touches[i] = null;
                processedThisFrame[i] = false;
            }
    }
}
