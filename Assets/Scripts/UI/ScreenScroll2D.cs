using UnityEngine;
using System.Collections;

public class ScreenScroll2D : MonoBehaviour {

	public static ScreenScroll2D Current;

    public Camera ScrollCamera;
    public int ScrollMouseButton;
    public float Speed;
    public Vector2 Multiplier = new Vector2(1,1);
    Transform _camT;

    void Awake()
    {
        Current = this;
    }

    public void Reset()
    {
        if (_camT != null)
            targetPosition = _camT.position = new Vector3(0, 0, _camT.position.z);
    }

	void Start () {
        _camT = ScrollCamera.transform;
        targetPosition = _camT.position;
	}

	void Update () {	
        if (Input.GetMouseButtonDown(0))
        {
            SavePosition(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            MoveToPosition(Input.mousePosition);
        }

        UpdatePosition();
	}

    Vector3 posDelta;
    Vector3 tmpPos;
    void UpdatePosition()
    {
        posDelta = (targetPosition - _camT.position).normalized * Speed;

        if (Mathf.Abs(posDelta.x) > Mathf.Abs(targetPosition.x - _camT.position.x))
            posDelta.x = targetPosition.x - _camT.position.x;

        if (Mathf.Abs(posDelta.y) > Mathf.Abs(targetPosition.y - _camT.position.y))
            posDelta.y = targetPosition.y - _camT.position.y;

        _camT.position = (_camT.position + posDelta);
    }

    Vector3 originalClick;
    Vector3 lastClick;
    void SavePosition(Vector3 pixelPos)
    {
        originalClick = ScrollCamera.ScreenToViewportPoint(pixelPos);
    }

    Vector3 targetPosition;
    void MoveToPosition(Vector3 pixelPos)
    {
        lastClick = ScrollCamera.ScreenToViewportPoint(pixelPos);

        targetPosition.x = _camT.position.x + ((originalClick.x - lastClick.x) * Multiplier.x);
        targetPosition.y = _camT.position.y + ((originalClick.y - lastClick.y) * Multiplier.y);
        targetPosition.z = _camT.position.z;
    }
}
