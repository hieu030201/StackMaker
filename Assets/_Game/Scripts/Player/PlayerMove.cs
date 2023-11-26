using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private static PlayerMove instance;
    public static PlayerMove Instance { get => instance; }

    [SerializeField] private float speed = 5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void Move(Direct directPlayer)
    {

        switch (directPlayer)
        {
            case Direct.Left:
                transform.Translate(Vector3.left * speed * Time.fixedDeltaTime, Space.World);
                break;
            case Direct.Right:
                transform.Translate(Vector3.right * speed * Time.fixedDeltaTime, Space.World);
                break;
            case Direct.Foward:
                transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime, Space.World);
                break;
            case Direct.Back:
                transform.Translate(Vector3.back * speed * Time.fixedDeltaTime, Space.World);
                break;
            default:
                break;
        }
    }
 
}
