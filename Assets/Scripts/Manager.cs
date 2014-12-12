using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Manager : MonoBehaviour
{
    static public Manager S { get {
        return GameObject.FindObjectOfType(typeof(Manager)) as Manager;
    }}

    // -- CONSTANTS --
    const int numBalls = 3;
    const int countdownLength = 3;
    static public int roundDuration = 20;

    private Dictionary<int, string> nameOfBall =
        new Dictionary<int, string>
    {
        {0, "Blue Ball"},
        {1, "Green Ball"},
        {2, "White Ball"}
    };

    private Dictionary<int, Color> colorOfBall =
        new Dictionary<int, Color>
    {
        {0, Color.blue},
        {1, Color.green},
        {2, Color.white}
    };

    // -- VARIABLES --
    public GameObject ground;
    public List<Ball> balls;

    public float roundStartTime;
    public GUIText countdownGUIText;

    public float timePassed {
        get {
            return Time.time - roundStartTime;
        }
    }

    public float roundProgress {
        get {
            return timePassed / (float)roundDuration;
        }
    }

    public bool countingDown {
        get {
            return (countdownLength - timePassed) > -1;
        }
    }

    void Start()
    {
        StartNewRound();
    }

    void CreateBalls()
    {
        float groundRadius = ground.transform.localScale.x / 2f;
        balls = Enumerable.Range(0, numBalls)
                          .Select(i => {
            float rad = ((float)i)/((float)numBalls) * 2f*Mathf.PI;
            Vector3 pos = new Vector3(Mathf.Sin(rad), 1f, Mathf.Cos(rad)) * (groundRadius-1);
            return CreateBall(pos, i);
        })
        .ToList();
    }

    Ball CreateBall(Vector3 pos, int number)
    {
        GameObject go = Instantiate(Resources.Load("Ball"), pos, Quaternion.identity) as GameObject;
        go.renderer.material.color = colorOfBall[number];
        Ball ball = go.GetComponent<Ball>();
        ball.number = number;
        return ball;
    }

    public void StartNewRound()
    {
        balls.ForEach(Destroy);
        roundStartTime = Time.time;
        CreateBalls();
        balls.ForEach(b => {
            b.frozen = true;
        });

        StartCoroutine(CountDownCoroutine());
    }

    public IEnumerator CountDownCoroutine()
    {
        GameObject countdownParent = Instantiate(Resources.Load("Countdown")) as GameObject;
        countdownGUIText = countdownParent.GetComponentInChildren<GUIText>();

        yield return new WaitForSeconds(countdownLength);
        balls.ForEach(b => b.frozen = false);

        yield return new WaitForSeconds(1);

        Destroy(countdownParent);
    }

    public void Update() {
        if (countingDown) {
            int countRemaining = countdownLength - (int)Mathf.Floor(Time.time - roundStartTime);
            countdownGUIText.text = countRemaining > 0 ? countRemaining.ToString() : "Start!";
        } else if (roundProgress >= 1f) {
            DetermineWinner();
        }
    }

    void DetermineWinner() {
        List<Ball> livingBalls = balls.Where(b => !b.dead).ToList();
        string winner = livingBalls.Count == 0 
                      ? "Ground"
                      : livingBalls.Select(b => nameOfBall[b.number])
                                   .Aggregate((acc, cur) => acc + " and " + cur);
        string suffix = livingBalls.Count > 1
                      ? " Win!"
                      : " Wins!";
        Debug.Log(winner + suffix);
        Application.LoadLevel(0);
    }
}
