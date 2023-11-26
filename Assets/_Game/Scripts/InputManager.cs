using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance { get => instance; }

    [SerializeField] private Vector3 startPos;
    public Vector3 StartPos { get => startPos; set { startPos = value; } }

    [SerializeField] private Vector3 endPos;
    public Vector3 EndPos { get => endPos; set { endPos = value; } }

    [SerializeField] private bool isTouching = false;
    public bool IsTouching { get => isTouching; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        GetPositionMouse();
    }

    private void GetPositionMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            isTouching = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            isTouching = false;
        }
    }
}
