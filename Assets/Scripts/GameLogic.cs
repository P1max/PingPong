using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private GameObject PrefabBall;
    [SerializeField] private GameObject Computer;
    [SerializeField] private GameObject Player;

    [SerializeField] private TextMeshProUGUI LeftScore;
    [SerializeField] private TextMeshProUGUI RightScore;
    [SerializeField] private TextMeshProUGUI ResultText;
        
    [SerializeField] private UnityEngine.UI.Button PlayButton;

    [SerializeField] private int scoreToWin = 10;
    [SerializeField] private int TimeTillTheEnd = 120;

    public static List<GameObject> BallList;

    private int leftscore = 0;
    private int rightscore = 0;

    void Start()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(Timer(TimeTillTheEnd));
        BallList = new List<GameObject>();
        StartCoroutine(WaitBallSpawn());    


    }
    public void ScoreLogic(bool leftSide, GameObject ball)
    {
        if (leftSide)
        {
            rightscore++;
            RightScore.text = rightscore.ToString();
        }
        else
        {
            leftscore++;
            LeftScore.text = leftscore.ToString();
        }

        if (leftscore == scoreToWin || rightscore == scoreToWin)
        {
            if (leftSide) ResultText.text = "You WIN";
            else ResultText.text = "You loose";
            ResultText.gameObject.SetActive(true);
            PlayButton.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

        StartCoroutine(DeleteBall(ball));
        if (BallList.Count == 0)
        {
            StartCoroutine(WaitBallSpawn());
        }

    }

    public IEnumerator WaitBallSpawn()
    {
        GameObject newball = Instantiate(PrefabBall);
        BallList.Add(newball);

        newball.transform.position = new Vector2(0, 0);
        BallMovement _ballMove = newball.GetComponent<BallMovement>();
        newball.SetActive(true);
        _ballMove.Speed = 0;

        yield return new WaitForSeconds(0.4f);

        int randomVal = Random.Range(0, 2) == 1 ? -1 : 1;
        _ballMove.Speed = _ballMove.Basespeed;
        _ballMove.direction = new Vector2(randomVal, Random.Range(-0.75f, 0.75f));

    }

    private IEnumerator Timer(int Time)
    {
        yield return new WaitForSeconds(Time);

        if (rightscore > leftscore) ResultText.text = "You WIN";
        else if (rightscore < leftscore) ResultText.text = "You loose";
        else ResultText.text = "DRAW";
        ResultText.gameObject.SetActive(true);
        PlayButton.gameObject.SetActive(true);
        UnityEngine.Time.timeScale = 0f;
    }

    private IEnumerator DeleteBall(GameObject ball)
    {
        BallList.Remove(ball);
        BallMovement move = ball.GetComponent<BallMovement>();
        move.Speed = 0;
        ball.transform.position = new Vector2(0, 50);

        yield return new WaitForSeconds(10);

        Destroy(ball);
    }
}