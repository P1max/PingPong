using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class ComputerMovement : MonoBehaviour
{
    [SerializeField] private GameObject Ball;
    public float MaxSpeed = 5f;    
    public Rigidbody2D Board;
    public GameObject RightTopCorner;
    public GameObject LeftBotCorner;


    BallMovement _ball;
    void Update()
    {
        GameObject temp = null;
        float distancex = float.MaxValue;
        Board.velocity = Vector2.zero;
        foreach (GameObject ball in GameLogic.BallList)
        {
            if(ball != null)
            {
                _ball = ball.GetComponent<BallMovement>();
                if(_ball.direction.x < 0)
                {
                    if((ball.transform.position.x - Board.transform.position.x) < distancex)
                    {
                        distancex = (ball.transform.position.x - Board.transform.position.x);
                        temp = ball;
                       
                    }
                }              
            }
        }
        if(temp != null)
        {
            Board.velocity = new Vector2(0, Mathf.Sign(temp.transform.position.y - Board.transform.position.y) * MaxSpeed);
        }
    }
} 