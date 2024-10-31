using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float Speed = 5f;
    public GameObject PlayerBoard;
    public GameObject LeftBotCorner;
    public GameObject RightTopCorner;
    [NonSerialized] public string UpButtonName = "UpArrow";
    [NonSerialized] public string DownButtonName = "DownArrow";

    private Rigidbody2D BoardRb;

    private void Start()
    {
        BoardRb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        //Movement
        Vector2 movement = Vector2.zero;
        if (Input.GetButton(UpButtonName))
        {
            movement.y = 1;
        }
        if (Input.GetButton(DownButtonName))
        {
            movement.y = -1;
        }
        BoardRb.velocity = movement * Speed;
        //


    }
}
