using System;
using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class BallMovement :  MonoBehaviour
{

    public GameObject Ball;
    public GameObject PlayerBoard;
    public GameObject ComputerBoard;
    public GameObject MainCamera;
    public GameObject LeftBotCorner;
    public GameObject RightTopCorner;
    public float Basespeed = 5f;
    public Vector2 direction;
    public float Speed;
    [SerializeField] private float SpeedScale = 0.5f;

    [SerializeField] Transform LeftBaseLine;
    [SerializeField] Transform RightBaseLine;

    [NonSerialized] public Rigidbody2D BallRb;
    [NonSerialized] public bool firsttouch = false;

    PlayerMovement Player;
    ComputerMovement Computer;

    private float localSizeY;
    private float angle = 0f;
    private bool playerlasttouch = true;

    private bool sizePlusPlayerCoroutineOn = false;
    private bool sizePlusComputerCoroutineOn = false;
    private bool sizeMinusPlayerCoroutineOn = false;
    private bool sizeMinusComputerCoroutineOn = false;
    private bool ControlInversionCoroutineOn = false;

    private Coroutine SizePlusPlayerCor;
    private Coroutine SizePlusComputerCor;
    private Coroutine SizeMinusPlayerCor;
    private Coroutine SizeMinusComputerCor;
    private Coroutine ControlInversionCor;

    private GameLogic score;
    void Awake()
    {
        PlayerBoard.TryGetComponent(out Player);
        ComputerBoard.TryGetComponent(out Computer);
        score = MainCamera.GetComponent<GameLogic>();
        BallRb = GetComponent<Rigidbody2D>();
        direction = new Vector2(1, UnityEngine.Random.Range(-0.75f, 0.75f));
        localSizeY = PlayerBoard.transform.localScale.y;
        Speed = Basespeed;

    }
    void Update()
    {
        BallRb.velocity = direction.normalized * Speed;
        if (Ball.transform.position.x > RightBaseLine.position.x)
        {
            score.ScoreLogic(false, Ball);

        }
        if (Ball.transform.position.x < LeftBaseLine.position.x)
        {
            score.ScoreLogic(true, Ball);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Wall>(out _))
        {
            direction.y = -direction.y;
        }
        if (collision.gameObject.TryGetComponent<PlayerMovement>(out _))
        {
            playerlasttouch = true;
            if (firsttouch == false)
            {
                firsttouch = true;
                Speed = Basespeed * 2;
            }
            else Speed += SpeedScale;
            if (RightTopCorner.transform.position.x > Player.LeftBotCorner.transform.position.x)
            {
                direction.y = -direction.y;
            }
            else
            {
                if (Ball.transform.position.y > collision.transform.position.y)
                {
                    angle = 2 * (LeftBotCorner.transform.position.y - collision.transform.position.y) / (Player.RightTopCorner.transform.position.y - Player.LeftBotCorner.transform.position.y);
                    if (angle > 0.9) angle = 0.9f;
                }
                else
                {
                    angle = -2 * (collision.transform.position.y - RightTopCorner.transform.position.y) / (Player.RightTopCorner.transform.position.y - Player.LeftBotCorner.transform.position.y);
                    if (angle > 0.9) angle = 0.9f;
                }

                direction.y = angle;

                direction.x = -direction.x;

            }
        }
        if (collision.gameObject.TryGetComponent<ComputerMovement>(out _))
        {
            playerlasttouch = false;
            if (firsttouch == false)
            {
                firsttouch = true;
                Speed = Basespeed * 2;
            }
            else Speed += SpeedScale;
            if (RightTopCorner.transform.position.x < Computer.LeftBotCorner.transform.position.x)
            {
                direction.y = -direction.y;
            }
            else
            {
                if (Ball.transform.position.y > collision.transform.position.y)
                {
                    angle = 2 * (LeftBotCorner.transform.position.y - collision.transform.position.y) / (Computer.RightTopCorner.transform.position.y - Computer.LeftBotCorner.transform.position.y);
                    if (angle > 0.9) angle = 0.9f;
                }
                else
                {
                    angle = -2 * (collision.transform.position.y - RightTopCorner.transform.position.y) / (Computer.RightTopCorner.transform.position.y - Computer.LeftBotCorner.transform.position.y);
                    if (angle > 0.9) angle = 0.9f;
                }
                direction.y = angle;
                direction.x = -direction.x;
            }

        }

        if (collision.gameObject.TryGetComponent<SizePlus>(out _)) //////////
        {
            Destroy(collision.gameObject);
            if (playerlasttouch == true)
            {
                if (sizeMinusPlayerCoroutineOn) StopCoroutine(SizeMinusPlayerCor);
                if(sizePlusPlayerCoroutineOn) StopCoroutine(SizePlusPlayerCor);
                PlayerBoard.transform.localScale = new Vector2(PlayerBoard.transform.localScale.x, localSizeY);
                SizePlusPlayerCor = StartCoroutine(SizePlus(PlayerBoard, playerlasttouch));
            }
            else
            {
                if (sizeMinusComputerCoroutineOn) StopCoroutine(SizeMinusComputerCor);
                if (sizePlusComputerCoroutineOn) StopCoroutine(SizePlusComputerCor);
                ComputerBoard.transform.localScale = new Vector2(ComputerBoard.transform.localScale.x, localSizeY);
                SizePlusComputerCor = StartCoroutine(SizePlus(ComputerBoard, playerlasttouch));
            }

        }
        if (collision.gameObject.TryGetComponent<SizeMinus>(out _)) ////////////////
        {
            Destroy(collision.gameObject);
            if (playerlasttouch == true)
            {
                if (sizeMinusPlayerCoroutineOn) StopCoroutine(SizeMinusPlayerCor);
                if (sizePlusPlayerCoroutineOn) StopCoroutine(SizePlusPlayerCor);
                PlayerBoard.transform.localScale = new Vector2(PlayerBoard.transform.localScale.x, localSizeY);
                SizeMinusPlayerCor = StartCoroutine(SizeMinus(PlayerBoard, playerlasttouch));
            }
            else
            {
                if (sizeMinusComputerCoroutineOn) StopCoroutine(SizeMinusComputerCor);
                if (sizePlusComputerCoroutineOn) StopCoroutine(SizePlusComputerCor);
                ComputerBoard.transform.localScale = new Vector2(ComputerBoard.transform.localScale.x, localSizeY);
                SizeMinusComputerCor = StartCoroutine(SizeMinus(ComputerBoard, playerlasttouch));
            }

        }
        if (collision.gameObject.TryGetComponent<ControlInversion>(out _))
        {
            Destroy(collision.gameObject);
            if (playerlasttouch == true)
            {
                if (ControlInversionCoroutineOn)
                {
                    StopCoroutine(ControlInversionCor);
                }
                ControlInversionCor = StartCoroutine(ControlInversion());
            }
        }
        if (collision.gameObject.TryGetComponent<BallClone>(out _))
        {
            Destroy(collision.gameObject);
            StartCoroutine(score.WaitBallSpawn());
        }

        IEnumerator SizePlus(GameObject obj, bool isplayer)
        {
            if (isplayer) sizePlusPlayerCoroutineOn = true;
            else sizePlusComputerCoroutineOn = true;

            obj.transform.localScale = new Vector2(obj.transform.localScale.x, localSizeY * 1.5f);

            yield return new WaitForSeconds(5f);

            obj.transform.localScale = new Vector2(obj.transform.localScale.x, localSizeY);

            if (isplayer) sizePlusPlayerCoroutineOn = false;
            else sizePlusComputerCoroutineOn = false;
        }
        IEnumerator SizeMinus(GameObject obj, bool isplayer)
        {
            if (isplayer) sizeMinusPlayerCoroutineOn = true;
            else sizeMinusComputerCoroutineOn = true;

            obj.transform.localScale = new Vector2(obj.transform.localScale.x, localSizeY / 1.5f);

            yield return new WaitForSeconds(5f);

            obj.transform.localScale = new Vector2(obj.transform.localScale.x, localSizeY);

            if (isplayer) sizeMinusPlayerCoroutineOn = false;
            else sizeMinusComputerCoroutineOn = false;
        }
        IEnumerator ControlInversion()
        {
            ControlInversionCoroutineOn = true;
            Player.UpButtonName = "DownArrow";
            Player.DownButtonName = "UpArrow";

            yield return new WaitForSeconds(5f);

            Player.UpButtonName = "UpArrow";
            Player.DownButtonName = "DownArrow";
            ControlInversionCoroutineOn = false;
        }

    }
}
