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
    static public int roundDuration = 25;

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
    public Ground ground;
    public List<Ball> balls;
    bool started = false;

    float roundStartTime = float.MaxValue;
    public GUIText countdownGUIText;

    public List<Ball> livingBalls {
        get {
            return balls.Where(b => !b.dead).ToList();
        }
    }

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
            return started && (countdownLength - timePassed) > -1;
        }
    }

    void Start()
    {
        ground = GameObject.Find("Ground").GetComponent<Ground>();
        countdownGUIText = GameObject.FindObjectOfType(typeof(GUIText)) as GUIText;
    }

    void CreateBalls()
    {
        float groundRadius = ground.transform.localScale.x / 2f;
        balls = Enumerable.Range(0, numBalls)
                          .Select(i => {
            float rad = ((float)i)/((float)numBalls) * 2f*Mathf.PI;
            Vector3 pos = new Vector3(Mathf.Sin(rad), 1f, Mathf.Cos(rad)) * (groundRadius-2);
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
        ground.frozen = true;

        StartCoroutine(CountDownCoroutine());
    }

    public IEnumerator CountDownCoroutine()
    {
        yield return new WaitForSeconds(countdownLength);
        balls.ForEach(b => b.frozen = false);
        ground.frozen = false;

        yield return new WaitForSeconds(1);

        countdownGUIText.text = "";
    }

    public void Update() {
        if (countingDown) {
            int countRemaining = countdownLength - (int)Mathf.Floor(Time.time - roundStartTime);
            countdownGUIText.text = countRemaining > 0 ? countRemaining.ToString() : "Start!";
        } else if (roundProgress >= 1f) {
            DetermineWinner();
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (started) {
                Reset();
            } else {
                started = true;
                StartNewRound();
            }
        }
    }

    void DetermineWinner() {
        string winner = livingBalls.Count == 0 
                      ? "Ground"
                      : livingBalls.Select(b => nameOfBall[b.number])
                                   .Aggregate((acc, cur) => acc + " and " + cur);
        string suffix = livingBalls.Count > 1
                      ? " Win!"
                      : " Wins!";
        countdownGUIText.text = winner + suffix;
        Time.timeScale = 0.1f;
        Invoke("Reset", 1f);
        Time.timeScale = 1f;
    }

    void Reset() {
        Application.LoadLevel(Application.loadedLevel);
    }
}
